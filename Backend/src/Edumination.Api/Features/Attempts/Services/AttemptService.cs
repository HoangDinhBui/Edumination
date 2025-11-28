using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Attempts.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using System.Text.Json;
using Edumination.Services.Interfaces;
using Education.Domain.Entities; // Chứa Asset, UserStats...
using Edumination.Api.Domain.Entities.Leaderboard;
using Edumination.Api.Domain.Enums;
using Edumination.Api.Domain.MongoEntities; // Namespace chứa SubmissionLog
using MongoDB.Driver;
using MongoDB.Bson; // Thư viện Mongo

namespace Edumination.Api.Features.Attempts.Services;

public class AttemptService : IAttemptService
{
    private readonly AppDbContext _db;
    private readonly IStorageService _storageService;
    private readonly IAssetService _assetService;
    private readonly ILogger<AttemptService> _logger;
    
    // Inject MongoDB Database (Thay vì Collection cứng)
    private readonly IMongoDatabase _mongoDb;
    private readonly Edumination.Api.Features.Attempts.Services.ISpeakingGradingService _speakingGradingService;

    public AttemptService(
        AppDbContext db, 
        IStorageService storageService, 
        IAssetService assetService, 
        ILogger<AttemptService> logger,
        IMongoDatabase mongoDb, // Inject Mongo Database
        Edumination.Api.Features.Attempts.Services.ISpeakingGradingService speakingGradingService)
    {
        _db = db;
        _storageService = storageService;
        _assetService = assetService;
        _logger = logger;
        _mongoDb = mongoDb;
        _speakingGradingService = speakingGradingService;
    }

    // Helper: Lấy Collection động theo Skill
    private IMongoCollection<SubmissionLog> GetCollectionBySkill(string skill)
    {
        string collectionName = $"{skill.ToLower()}_submissions";
        return _mongoDb.GetCollection<SubmissionLog>(collectionName);
    }

    public async Task<StartAttemptResponse> StartAsync(long userId, StartAttemptRequest req, CancellationToken ct)
    {
        // Giữ nguyên logic hiện tại
        var published = await _db.TestPapers.AnyAsync(p => p.Id == req.PaperId && p.Status == "PUBLISHED", ct);
        if (!published) throw new InvalidOperationException("Paper not published");

        var nextNo = await _db.TestAttempts
            .CountAsync(a => a.UserId == userId && a.PaperId == req.PaperId, ct) + 1;

        var attempt = new TestAttempt
        {
            UserId = userId,
            PaperId = req.PaperId,
            AttemptNo = nextNo,
            StartedAt = DateTime.UtcNow,
            Status = "IN_PROGRESS"
        };
        _db.TestAttempts.Add(attempt);
        await _db.SaveChangesAsync(ct);

        var sections = await _db.TestSections
            .Where(s => s.PaperId == req.PaperId && s.IsPublished)
            .Select(s => new { s.Id, s.Skill, s.TimeLimitSec })
            .ToListAsync(ct);

        foreach (var s in sections)
        {
            _db.SectionAttempts.Add(new SectionAttempt
            {
                TestAttemptId = attempt.Id,
                SectionId = s.Id,
                StartedAt = DateTime.UtcNow,
                FinishedAt = null,
                RawScore = null,
                ScaledBand = null,
                Status = "IN_PROGRESS"
            });
        }
        await _db.SaveChangesAsync(ct);

        return new StartAttemptResponse(
            attempt.Id,
            sections.Select(s => new SectionSummary(s.Id, s.Skill, s.TimeLimitSec))
        );
    }

    // --- HÀM SUBMIT ANSWER ĐÃ ĐƯỢC NÂNG CẤP ---
    public async Task<SubmitAnswerResponse> SubmitAnswerAsync(long attemptId, long sectionId, SubmitAnswerRequest request, long userId, CancellationToken ct)
    {
        // 1. Kiểm tra TestAttempt
        var testAttempt = await _db.TestAttempts
            .FirstOrDefaultAsync(ta => ta.Id == attemptId && ta.UserId == userId && ta.Status != "CANCELLED", ct);
        if (testAttempt == null)
            throw new InvalidOperationException("Test attempt not found or not accessible.");

        // 2. Kiểm tra SectionAttempt (Kèm TestSection để lấy Skill)
        var sectionAttempt = await _db.SectionAttempts
            .Include(sa => sa.TestAttempt)
            .Include(sa => sa.TestSection) // Include bảng này để biết Skill
            .FirstOrDefaultAsync(sa => sa.TestAttemptId == attemptId && sa.SectionId == sectionId && sa.Status != "CANCELLED", ct);
        
        if (sectionAttempt == null)
            throw new InvalidOperationException("Section attempt not found or not accessible.");

        // 3. Kiểm tra Question
        var question = await _db.Questions
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId && q.SectionId == sectionId, ct);
        if (question == null)
            throw new InvalidOperationException("Question not found.");

        // --- KHỞI TẠO BIẾN CHẤM ĐIỂM ---
        bool? isCorrect = null;
        decimal? earnedScore = null;

        // 4. Lấy dữ liệu đáp án đúng từ Database
        var mcqCorrectChoice = await _db.QuestionChoices
            .FirstOrDefaultAsync(c => c.QuestionId == request.QuestionId && c.IsCorrect, ct);

        var answerKey = await _db.QuestionAnswerKeys
            .FirstOrDefaultAsync(ak => ak.QuestionId == request.QuestionId, ct);

        // --- LOGIC CHẤM ĐIỂM ---
        
        // CASE A: Chấm MCQ (Trắc nghiệm)
        if (mcqCorrectChoice != null)
        {
            try
            {
                using var doc = JsonDocument.Parse(request.AnswerJson.ToString());
                if (doc.RootElement.TryGetProperty("choice_id", out var choiceIdElement))
                {
                    long userChoiceId = choiceIdElement.GetInt64();
                    isCorrect = (userChoiceId == mcqCorrectChoice.Id);
                }
            }
            catch
            {
                isCorrect = false;
            }
        }
        // CASE B: Chấm Fill-Blank (Điền từ)
        else if (answerKey != null)
        {
            var keyJson = JsonSerializer.Deserialize<object>(answerKey.KeyJson);
            var userAnswer = request.AnswerJson;
            isCorrect = IsAnswerCorrect(keyJson, userAnswer);
        }

        // --- TÍNH ĐIỂM SỐ ---
        if (isCorrect == true)
        {
            try
            {
                earnedScore = 1.0m; // Mặc định 1 điểm
                if (!string.IsNullOrEmpty(question.MetaJson))
                {
                    var meta = JsonSerializer.Deserialize<Dictionary<string, object>>(question.MetaJson);
                    if (meta != null && meta.TryGetValue("max_score", out var maxScoreObj))
                    {
                        if (decimal.TryParse(maxScoreObj.ToString(), out decimal parsedScore))
                        {
                            earnedScore = parsedScore;
                        }
                    }
                }
            }
            catch
            {
                earnedScore = 1.0m;
            }
        }
        else
        {
            earnedScore = 0.0m;
        }

        // --- LƯU VÀO MONGODB (Động theo Skill) ---
        
        // 1. Chọn Collection đúng (reading_submissions hoặc listening_submissions)
        var targetCollection = GetCollectionBySkill(sectionAttempt.TestSection.Skill);

        // 2. Chuẩn bị dữ liệu
        var bsonAnswer = BsonDocument.Parse(request.AnswerJson.ToString());
        var answerLog = new StudentAnswerLog
        {
            QuestionId = request.QuestionId,
            AnswerJson = bsonAnswer,
            IsCorrect = isCorrect,
            EarnedScore = earnedScore,
            AnsweredAt = DateTime.UtcNow
        };

        var filter = Builders<SubmissionLog>.Filter.Eq(s => s.SectionAttemptId, sectionAttempt.Id);

        // 3. Upsert Document cha (Nếu chưa có thì tạo mới)
        var updateInit = Builders<SubmissionLog>.Update
            .SetOnInsert(s => s.UserId, userId)
            .SetOnInsert(s => s.PaperId, sectionAttempt.TestAttempt.PaperId)
            .SetOnInsert(s => s.SectionId, sectionId)
            .Set(s => s.LastUpdatedAt, DateTime.UtcNow);

        await targetCollection.UpdateOneAsync(filter, updateInit, new UpdateOptions { IsUpsert = true }, ct);

        // 4. Cập nhật câu trả lời (Xóa cũ -> Thêm mới)
        var updatePull = Builders<SubmissionLog>.Update
            .PullFilter(s => s.Answers, a => a.QuestionId == request.QuestionId);
        await targetCollection.UpdateOneAsync(filter, updatePull, cancellationToken: ct);

        var updatePush = Builders<SubmissionLog>.Update
            .Push(s => s.Answers, answerLog);
        await targetCollection.UpdateOneAsync(filter, updatePush, cancellationToken: ct);

        return new SubmitAnswerResponse(0, request.QuestionId, isCorrect, earnedScore);
    }

    // Hàm này đã được nâng cấp để Parse JSON lấy giá trị bên trong
    private bool IsAnswerCorrect(object keyJson, object userAnswer)
    {
        string ExtractText(object jsonObj)
        {
            if (jsonObj == null) return "";
            var str = jsonObj.ToString();
            try
            {
                // Cố gắng đọc json để lấy giá trị của 'text_answer'
                using var doc = JsonDocument.Parse(str);
                if (doc.RootElement.ValueKind == JsonValueKind.Object && 
                    doc.RootElement.TryGetProperty("text_answer", out var val))
                {
                    return val.ToString().Trim().ToLower();
                }
                // Nếu không đúng định dạng object thì trả về chuỗi gốc
                return str.Trim().ToLower();
            }
            catch
            {
                return str.Trim().ToLower();
            }
        }

        var keyText = ExtractText(keyJson);   // Sẽ lấy ra "ha"
        var userText = ExtractText(userAnswer); // Sẽ lấy ra "ha"

        return keyText == userText; // "ha" == "ha" -> True
    }

    public async Task<SubmitSectionResponse> SubmitSectionAsync(long attemptId, long sectionId, SubmitSectionRequest request, long userId, CancellationToken ct)
    {
        if (!request.ConfirmSubmission)
            throw new InvalidOperationException("Submission confirmation is required.");

        var testAttempt = await _db.TestAttempts
            .FirstOrDefaultAsync(ta => ta.Id == attemptId && ta.UserId == userId && ta.Status != "CANCELLED", ct);
        if (testAttempt == null)
            throw new InvalidOperationException("Test attempt not found or not accessible.");

        var sectionAttempt = await _db.SectionAttempts
            .Include(sa => sa.TestAttempt)
            .Include(sa => sa.TestSection) // Include để lấy Skill
            .FirstOrDefaultAsync(sa => sa.TestAttemptId == attemptId && sa.SectionId == sectionId && sa.Status != "CANCELLED", ct);
        if (sectionAttempt == null)
            throw new InvalidOperationException("Section attempt not found or not accessible.");

        // --- LẤY BÀI LÀM TỪ MONGODB (Theo Collection động) ---
        var targetCollection = GetCollectionBySkill(sectionAttempt.TestSection.Skill);
        
        var submissionLog = await targetCollection
            .Find(s => s.SectionAttemptId == sectionAttempt.Id)
            .FirstOrDefaultAsync(ct);

        var answers = submissionLog?.Answers ?? new List<StudentAnswerLog>();

        decimal rawScore = 0;
        var section = sectionAttempt.TestSection;
        
        if (section.Skill == "LISTENING" || section.Skill == "READING")
        {
            // Tính tổng điểm từ các câu trả lời đã lưu
            if (answers.Any())
            {
                rawScore = answers
                    .Where(a => a.IsCorrect == true)
                    .Sum(a => a.EarnedScore ?? 0);
            }
            sectionAttempt.RawScore = rawScore;
        }
        else
        {
            sectionAttempt.RawScore = null; // Writing/Speaking chấm sau
        }

        sectionAttempt.Status = "SUBMITTED";
        sectionAttempt.FinishedAt = DateTime.UtcNow;

        // Quy đổi Band Score
        if (sectionAttempt.RawScore.HasValue && (section.Skill == "LISTENING" || section.Skill == "READING"))
        {
            var bandScale = await _db.BandScales
                .Where(bs => bs.PaperId == testAttempt.PaperId && bs.Skill == section.Skill)
                .OrderBy(bs => Math.Abs(bs.RawMin - (int)sectionAttempt.RawScore.Value))
                .FirstOrDefaultAsync(ct);
            
            if (bandScale != null)
            {
                sectionAttempt.ScaledBand = bandScale.Band;
            }
        }

        await _db.SaveChangesAsync(ct);

        return new SubmitSectionResponse(sectionAttempt.Id, sectionAttempt.RawScore, sectionAttempt.ScaledBand, sectionAttempt.Status);
    }

    // Các hàm SubmitSpeakingAsync, SubmitWritingAsync, SubmitTestAsync, UpdateUserStatsAsync, UpdateLeaderboardAsync giữ nguyên như cũ...
    // (Tôi lược bỏ bớt để tiết kiệm không gian, bạn giữ nguyên code cũ của các hàm này nhé)
    
    public async Task<SubmitSpeakingResponse> SubmitSpeakingAsync(long attemptId, long sectionId, SubmitSpeakingRequest request, long userId, CancellationToken ct)
    {
        // ... (Giữ nguyên code cũ) ...
         // Validate request
        if (request == null || request.AudioFile == null || !request.ConfirmSubmission)
            throw new InvalidOperationException("Audio file and submission confirmation are required.");

        // Kiểm tra TestAttempt
        var testAttempt = await _db.TestAttempts
            .FirstOrDefaultAsync(ta => ta.Id == attemptId && ta.UserId == userId && ta.Status != "CANCELLED", ct);
        if (testAttempt == null)
            throw new InvalidOperationException("Test attempt not found or not accessible.");

        // Kiểm tra SectionAttempt
        var sectionAttempt = await _db.SectionAttempts
            .FirstOrDefaultAsync(sa => sa.TestAttemptId == attemptId && sa.SectionId == sectionId && sa.Status != "CANCELLED", ct);
        if (sectionAttempt == null)
            throw new InvalidOperationException("Section attempt not found or not accessible.");

        // Kiểm tra Section là Speaking
        var section = await _db.TestSections
            .FirstOrDefaultAsync(s => s.Id == sectionId && s.Skill == "SPEAKING", ct);
        if (section == null)
            throw new InvalidOperationException("Section is not a Speaking section.");

        // Upload file audio
        var fileExtension = Path.GetExtension(request.AudioFile.FileName).ToLower();
        if (!new[] { ".mp3", ".wav", ".m4a" }.Contains(fileExtension))
            throw new InvalidOperationException("Unsupported audio format. Only MP3, WAV, and M4A are allowed.");

        var fileName = $"speaking/{userId}/{attemptId}/{sectionId}/{Guid.NewGuid()}{fileExtension}";
        using var stream = request.AudioFile.OpenReadStream();
        var storageUrl = await _storageService.UploadAsync(stream, fileName, request.AudioFile.ContentType, ct);

        // Lưu thông tin asset
        var asset = new Asset
        {
            Kind = "AUDIO",
            StorageUrl = storageUrl,
            MediaType = request.AudioFile.ContentType,
            ByteSize = request.AudioFile.Length,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            LanguageCode = "en" 
        };
        _db.Assets.Add(asset);
        await _db.SaveChangesAsync(ct);

        // Lưu Speaking submission
        var speakingSubmission = new SpeakingSubmission
        {
            SectionAttemptId = sectionAttempt.Id,
            PromptText = request.PromptText,
            AudioAssetId = asset.Id,
            CreatedAt = DateTime.UtcNow
        };
        _db.SpeakingSubmissions.Add(speakingSubmission);

        // Cập nhật trạng thái SectionAttempt
        sectionAttempt.Status = "SUBMITTED";
        sectionAttempt.FinishedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        // Process grading asynchronously (blocking here for simplicity)
        try
        {
            var grading = await _speakingGradingService.ProcessSpeakingSubmissionAsync(storageUrl, ct);

            // Save grading results
            speakingSubmission.AsrText = grading.TranscribedText;
            speakingSubmission.WordsCount = grading.WordCount;
            speakingSubmission.FluencyScore = grading.FluencyScore;
            speakingSubmission.LexicalScore = grading.LexicalScore;
            speakingSubmission.GrammarScore = grading.GrammarScore;
            speakingSubmission.PronunciationScore = grading.PronunciationScore;
            speakingSubmission.OverallBand = grading.OverallBand;
            speakingSubmission.AiFeedback = grading.Feedback;
            speakingSubmission.GradedAt = DateTime.UtcNow;

            // Update section attempt scaled band based on AI overall
            sectionAttempt.ScaledBand = grading.OverallBand;

            await _db.SaveChangesAsync(ct);

            return new SubmitSpeakingResponse(
                speakingSubmission.Id,
                sectionAttempt.Id,
                asset.Id,
                sectionAttempt.Status)
            {
                OverallBand = grading.OverallBand,
                Transcript = grading.TranscribedText,
                Feedback = grading.Feedback
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while grading speaking submission");
            // Return without grading
            return new SubmitSpeakingResponse(
                speakingSubmission.Id,
                sectionAttempt.Id,
                asset.Id,
                sectionAttempt.Status
            );
        }
    }

    public async Task<SubmitWritingResponse> SubmitWritingAsync(long attemptId, long sectionId, SubmitWritingRequest request, long userId, CancellationToken ct)
    {
        // ... (Giữ nguyên code cũ) ...
         _logger.LogInformation("Submitting writing for attemptId: {AttemptId}, sectionId: {SectionId}, userId: {UserId}", attemptId, sectionId, userId);

        // Validate request
        if (request == null || string.IsNullOrWhiteSpace(request.ContentText) || !request.ConfirmSubmission)
            throw new InvalidOperationException("Text content and submission confirmation are required.");

        // Kiểm tra TestAttempt
        var testAttempt = await _db.TestAttempts
            .FirstOrDefaultAsync(ta => ta.Id == attemptId && ta.UserId == userId && ta.Status != "CANCELLED", ct);
        if (testAttempt == null)
            throw new InvalidOperationException("Test attempt not found or not accessible.");
        
        // Kiểm tra SectionAttempt
        var sectionAttempt = await _db.SectionAttempts
            .FirstOrDefaultAsync(sa => sa.TestAttemptId == attemptId && sa.SectionId == sectionId && sa.Status != "CANCELLED", ct);
        if (sectionAttempt == null)
            throw new InvalidOperationException("Section attempt not found or not accessible.");

        // Kiểm tra Section là Writing
        var section = await _db.TestSections
            .FirstOrDefaultAsync(s => s.Id == sectionId && s.Skill == "WRITING", ct);
        if (section == null)
            throw new InvalidOperationException("Section is not a Writing section.");

        // Validate file format (nếu có file)
        long? assetId = null;
        if (request.File != null)
        {
            var fileExtension = Path.GetExtension(request.File.FileName).ToLower();
            if (!new[] { ".pdf", ".jpg", ".jpeg", ".png" }.Contains(fileExtension))
                throw new InvalidOperationException("Unsupported file format. Only PDF, JPG, and PNG are allowed.");

            // Generate file path and save file
            var filePath = await _storageService.GenerateUploadPathAsync(request.File.ContentType, request.File.Length, ct);
            using var stream = request.File.OpenReadStream();
            var storageUrl = await _storageService.SaveFileAsync(filePath, stream, ct);

            // Lưu thông tin asset
            var asset = new Asset
            {
                Kind = fileExtension == ".pdf" ? "DOC" : "IMAGE", 
                StorageUrl = storageUrl,
                MediaType = request.File.ContentType,
                ByteSize = request.File.Length,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                Sha256 = null, 
                LanguageCode = null, 
                DurationSec = null 
            };
            _db.Assets.Add(asset);
            await _db.SaveChangesAsync(ct);
            assetId = asset.Id;
        }

        // Lưu Writing submission
        var writingSubmission = new WritingSubmission
        {
            SectionAttemptId = sectionAttempt.Id,
            ContentText = request.ContentText,
            PromptText = request.PromptText,
            CreatedAt = DateTime.UtcNow
        };
        _db.WritingSubmissions.Add(writingSubmission);

        // Cập nhật trạng thái SectionAttempt
        sectionAttempt.Status = "SUBMITTED";
        sectionAttempt.FinishedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Writing submitted successfully: submissionId: {SubmissionId}", writingSubmission.Id);

        return new SubmitWritingResponse(
            writingSubmission.Id,
            sectionAttempt.Id,
            assetId,
            sectionAttempt.Status
        );
    }

    public async Task<SubmitTestResponse> SubmitTestAsync(long attemptId, SubmitTestRequest request, long userId, CancellationToken ct)
    {
        // ... (Giữ nguyên code cũ) ...
         _logger.LogInformation("Submitting test attemptId: {AttemptId}, userId: {UserId}", attemptId, userId);

        // Validate request
        if (!request.ConfirmSubmission)
            throw new InvalidOperationException("Submission confirmation is required.");

        // Kiểm tra TestAttempt
        var testAttempt = await _db.TestAttempts
            .Include(ta => ta.SectionAttempts) // Load sections để kiểm tra
                .ThenInclude(sa => sa.TestSection)
            .FirstOrDefaultAsync(ta => ta.Id == attemptId && ta.UserId == userId && ta.Status == "IN_PROGRESS", ct);
        if (testAttempt == null)
            throw new InvalidOperationException("Test attempt not found, not owned by user, or not in progress.");

        // Kiểm tra tất cả sections đã được nộp
        var unsubmittedSections = testAttempt.SectionAttempts
            .Where(sa => sa.Status != "SUBMITTED" && sa.Status != "GRADED")
            .ToList();
        if (unsubmittedSections.Any())
        {
            var sectionNames = string.Join(", ", unsubmittedSections.Select(sa => sa.TestSection.Skill));
            throw new InvalidOperationException($"All sections must be submitted. Pending: {sectionNames}");
        }

        // Cập nhật TestAttempt status
        testAttempt.Status = "SUBMITTED";
        testAttempt.FinishedAt = DateTime.UtcNow;

        // Tính overall band từ view v_test_attempt_band
        var overallBandResult = await _db.Set<vTestAttemptBand>()
            .FromSqlRaw("SELECT * FROM v_test_attempt_band WHERE test_attempt_id = {0}", attemptId)
            .FirstOrDefaultAsync(ct);
        var overallBand = overallBandResult?.OverallBand ?? 0m;

        // Lấy section bands
        var sectionBands = await _db.SectionAttempts
            .Where(sa => sa.TestAttemptId == attemptId)
            .Include(sa => sa.TestSection)
            .Select(sa => new SectionBandSummary
            {
                Skill = sa.TestSection.Skill,
                RawScore = sa.RawScore,
                BandScore = sa.ScaledBand
            })
            .ToListAsync(ct);

        // Cập nhật UserStats (best/worst band, avg skills)
        await UpdateUserStatsAsync(userId, testAttempt.PaperId, overallBand, sectionBands, ct);

        // Cập nhật Leaderboard (nếu là best attempt cho paper này)
        var isBestAttempt = await UpdateLeaderboardAsync(userId, testAttempt.PaperId, overallBand, ct);

        // Lưu thay đổi
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Test submitted successfully: attemptId: {AttemptId}, overallBand: {OverallBand}", attemptId, overallBand);

        return new SubmitTestResponse(attemptId, overallBand, sectionBands, testAttempt.Status, isBestAttempt);
    }
    
    // Helper: Cập nhật UserStats (Giữ nguyên)
    private async Task UpdateUserStatsAsync(long userId, long paperId, decimal overallBand, IEnumerable<SectionBandSummary> sectionBands, CancellationToken ct)
    {
         var userStats = await _db.UserStats.FindAsync(userId);
        if (userStats == null)
        {
            userStats = new UserStats { UserId = userId };
            _db.UserStats.Add(userStats);
        }

        userStats.TotalTests++;
        userStats.BestBand = Math.Max(userStats.BestBand ?? 0, overallBand);
        userStats.WorstBand = userStats.WorstBand == null ? overallBand : Math.Min(userStats.WorstBand.Value, overallBand);

        // Tính avg cho từng skill
        var listeningBand = sectionBands.FirstOrDefault(s => s.Skill == "LISTENING")?.BandScore ?? 0;
        var readingBand = sectionBands.FirstOrDefault(s => s.Skill == "READING")?.BandScore ?? 0;
        var writingBand = sectionBands.FirstOrDefault(s => s.Skill == "WRITING")?.BandScore ?? 0;
        var speakingBand = sectionBands.FirstOrDefault(s => s.Skill == "SPEAKING")?.BandScore ?? 0;

        userStats.AvgListeningBand = ((userStats.AvgListeningBand * (userStats.TotalTests - 1)) + listeningBand) / userStats.TotalTests;
        userStats.AvgReadingBand = ((userStats.AvgReadingBand * (userStats.TotalTests - 1)) + readingBand) / userStats.TotalTests;
        userStats.AvgWritingBand = ((userStats.AvgWritingBand * (userStats.TotalTests - 1)) + writingBand) / userStats.TotalTests;
        userStats.AvgSpeakingBand = ((userStats.AvgSpeakingBand * (userStats.TotalTests - 1)) + speakingBand) / userStats.TotalTests;

        // Cập nhật best/worst skill (logic đơn giản, có thể phức tạp hơn)
        var bestSkillStr = sectionBands.OrderByDescending(s => s.BandScore).FirstOrDefault()?.Skill;
        var worstSkillStr = sectionBands.OrderBy(s => s.BandScore).FirstOrDefault()?.Skill;

        userStats.BestSkill = Enum.TryParse<Skill>(bestSkillStr, out var bestSkill)
            ? bestSkill
            : (Skill?)null;

        userStats.WorstSkill = Enum.TryParse<Skill>(worstSkillStr, out var worstSkill)
            ? worstSkill
            : (Skill?)null;

        userStats.UpdatedAt = DateTime.UtcNow;
    }

    // Helper: Cập nhật Leaderboard (Giữ nguyên)
    private async Task<bool> UpdateLeaderboardAsync(long userId, long paperId, decimal overallBand, CancellationToken ct)
    {
         var existingEntry = await _db.LeaderboardEntries
            .FirstOrDefaultAsync(le => le.UserId == userId && le.PaperId == paperId, ct);

        if (existingEntry == null || overallBand > existingEntry.BestOverallBand)
        {
            if (existingEntry == null)
            {
                existingEntry = new LeaderboardEntry
                {
                    UserId = userId,
                    PaperId = paperId,
                    BestOverallBand = overallBand,
                    BestAt = DateTime.UtcNow
                };
                _db.LeaderboardEntries.Add(existingEntry);
            }
            else
            {
                existingEntry.BestOverallBand = overallBand;
                existingEntry.BestAt = DateTime.UtcNow;
            }
            return true; // Đây là best attempt
        }
        return false;
    }

    public async Task<SectionResultDto> GetSectionResultAsync(long attemptId, long sectionId, long userId, CancellationToken ct)
{
    // 1. Lấy thông tin từ MySQL (Kèm đáp án đúng)
    var sectionAttempt = await _db.SectionAttempts
        .AsNoTracking()
        .Include(sa => sa.TestAttempt).ThenInclude(ta => ta.User)
        .Include(sa => sa.TestAttempt).ThenInclude(ta => ta.TestPaper)
        .Include(sa => sa.TestSection).ThenInclude(ts => ts.Passages).ThenInclude(p => p.Questions).ThenInclude(q => q.QuestionChoices)
        .Include(sa => sa.TestSection).ThenInclude(ts => ts.Passages).ThenInclude(p => p.Questions).ThenInclude(q => q.QuestionAnswerKey)
        .FirstOrDefaultAsync(sa => sa.TestAttemptId == attemptId && sa.SectionId == sectionId, ct);

    if (sectionAttempt == null) throw new InvalidOperationException("Không tìm thấy bài làm.");

    // 2. Lấy bài làm từ MongoDB
    var targetCollection = GetCollectionBySkill(sectionAttempt.TestSection.Skill);
    var log = await targetCollection.Find(s => s.SectionAttemptId == sectionAttempt.Id).FirstOrDefaultAsync(ct);
    var userAnswers = log?.Answers ?? new List<StudentAnswerLog>();

    // 3. Tính toán thời gian làm bài
    var timeSpan = (sectionAttempt.FinishedAt - sectionAttempt.StartedAt) ?? TimeSpan.Zero;
    string timeTaken = $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:D2}";

    // 4. Tổng hợp câu hỏi
    var questionsResult = new List<QuestionResultDto>();
    int totalQuestions = 0;

    foreach (var passage in sectionAttempt.TestSection.Passages)
    {
        foreach (var q in passage.Questions)
        {
            totalQuestions++;
            var qDto = new QuestionResultDto
            {
                Id = q.Id,
                Position = q.Position,
                PartTitle = passage.Title ?? "Unknown Part", // Dùng Title của Passage làm tên Part
                IsCorrect = false,
                UserAnswerText = "Not Answered",
                CorrectAnswerText = ""
            };

            // --- LẤY ĐÁP ÁN ĐÚNG ---
            if (q.Qtype == "MCQ" && q.QuestionChoices != null)
            {
                var correct = q.QuestionChoices.FirstOrDefault(c => c.IsCorrect);
                qDto.CorrectAnswerText = correct?.Content ?? "N/A";
            }
            else if (q.QuestionAnswerKey != null)
            {
                 // Parse JSON lấy text_answer
                 try {
                    using var doc = JsonDocument.Parse(q.QuestionAnswerKey.KeyJson);
                    if(doc.RootElement.TryGetProperty("text_answer", out var val)) qDto.CorrectAnswerText = val.ToString();
                 } catch {}
            }

            // --- LẤY ĐÁP ÁN USER (TỪ MONGO) ---
            var userLog = userAnswers.FirstOrDefault(ua => ua.QuestionId == q.Id);
            if (userLog != null)
            {
                qDto.IsCorrect = userLog.IsCorrect ?? false;
                
                // Parse Bson lấy text hiển thị
                try {
                    using var doc = JsonDocument.Parse(userLog.AnswerJson.ToString());
                    if (doc.RootElement.TryGetProperty("choice_id", out var cid))
                    {
                        // Nếu là MCQ, map ID sang Content
                        var choice = q.QuestionChoices.FirstOrDefault(c => c.Id == cid.GetInt64());
                        qDto.UserAnswerText = choice?.Content ?? "Unknown";
                    }
                    else if (doc.RootElement.TryGetProperty("text_answer", out var txt))
                    {
                        qDto.UserAnswerText = txt.ToString();
                    }
                } catch {}
            }

            questionsResult.Add(qDto);
        }
    }

    return new SectionResultDto
    {
        PaperTitle = sectionAttempt.TestAttempt.TestPaper.Title,
        CandidateName = sectionAttempt.TestAttempt.User.FullName,
        AvatarUrl = sectionAttempt.TestAttempt.User.AvatarUrl,
        BandScore = sectionAttempt.ScaledBand?.ToString("0.0") ?? "0.0",
        RawScore = $"{(int)(sectionAttempt.RawScore ?? 0)}/{totalQuestions}",
        TimeTaken = timeTaken,
        Questions = questionsResult.OrderBy(q => q.Position).ToList()
    };
}
}