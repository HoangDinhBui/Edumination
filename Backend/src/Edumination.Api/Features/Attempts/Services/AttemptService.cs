using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Attempts.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using System.Text.Json;
using Edumination.Services.Interfaces;
using Education.Domain.Entities;
using Edumination.Api.Domain.Entities.Leaderboard;
using Edumination.Api.Domain.Enums;

namespace Edumination.Api.Features.Attempts.Services;

public class AttemptService : IAttemptService
{
    private readonly AppDbContext _db;
    private readonly IStorageService _storageService; // Dịch vụ lưu trữ file (S3, GCS, etc.)
    private readonly IAssetService _assetService; // Dịch vụ quản lý assets
    private readonly ILogger<AttemptService> _logger;
    public AttemptService(AppDbContext db, IStorageService storageService, IAssetService assetService, ILogger<AttemptService> logger)
    {
        _db = db;
        _storageService = storageService;
        _assetService = assetService;
        _logger = logger;
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

    public async Task<SubmitAnswerResponse> SubmitAnswerAsync(long attemptId, long sectionId, SubmitAnswerRequest request, long userId, CancellationToken ct)
    {
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

        // Kiểm tra Question
        var question = await _db.Questions
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId && q.SectionId == sectionId, ct);
        if (question == null)
            throw new InvalidOperationException("Question not found.");

        // Tạo và lưu Answer
        var answer = new Answer
        {
            SectionAttemptId = sectionAttempt.Id,
            QuestionId = request.QuestionId,
            AnswerJson = JsonSerializer.Serialize(request.AnswerJson)
        };

        // Chấm điểm tự động cho L/R
        var answerKey = await _db.QuestionAnswerKeys
            .FirstOrDefaultAsync(ak => ak.QuestionId == request.QuestionId, ct);
        if (answerKey != null)
        {
            var keyJson = JsonSerializer.Deserialize<object>(answerKey.KeyJson);
            var userAnswer = request.AnswerJson;
            if (IsAnswerCorrect(keyJson, userAnswer))
            {
                answer.IsCorrect = true;
                try
                {
                    answer.EarnedScore = question.MetaJson != null
                        ? JsonSerializer.Deserialize<Dictionary<string, object>>(question.MetaJson)?
                            .GetValueOrDefault("max_score") as decimal? ?? 1.0m
                        : 1.0m;
                }
                catch (JsonException)
                {
                    answer.EarnedScore = 1.0m; // Giá trị mặc định khi JSON không hợp lệ
                }
            }
            else
            {
                answer.IsCorrect = false;
                answer.EarnedScore = 0.0m;
            }
            answer.CheckedAt = DateTime.UtcNow;
        }

        _db.Answers.Add(answer);
        await _db.SaveChangesAsync(ct);

        return new SubmitAnswerResponse(answer.Id, answer.QuestionId, answer.IsCorrect, answer.EarnedScore);
    }

    private bool IsAnswerCorrect(object keyJson, object userAnswer)
    {
        var keyStr = keyJson?.ToString()?.ToLower();
        var userStr = userAnswer?.ToString()?.ToLower();
        return keyStr == userStr;
    }

    public async Task<SubmitSectionResponse> SubmitSectionAsync(long attemptId, long sectionId, SubmitSectionRequest request, long userId, CancellationToken ct)
    {
        // Validate request
        if (!request.ConfirmSubmission)
            throw new InvalidOperationException("Submission confirmation is required.");

        // Retrieve the test attempt
        var testAttempt = await _db.TestAttempts
            .FirstOrDefaultAsync(ta => ta.Id == attemptId && ta.UserId == userId && ta.Status != "CANCELLED", ct);
        if (testAttempt == null)
            throw new InvalidOperationException("Test attempt not found or not accessible.");

        // Retrieve the section attempt
        var sectionAttempt = await _db.SectionAttempts
            .FirstOrDefaultAsync(sa => sa.TestAttemptId == attemptId && sa.SectionId == sectionId && sa.Status != "CANCELLED", ct);
        if (sectionAttempt == null)
            throw new InvalidOperationException("Section attempt not found or not accessible.");

        // Retrieve all answers for this section attempt
        var answers = await _db.Answers
            .Where(a => a.SectionAttemptId == sectionAttempt.Id)
            .ToListAsync(ct);

        if (!answers.Any())
            throw new InvalidOperationException("No answers submitted for this section.");

        // Calculate raw score for objective sections (e.g., Listening/Reading)
        decimal rawScore = 0;
        var section = await _db.TestSections
            .FirstAsync(s => s.Id == sectionId, ct);
        if (section.Skill == "LISTENING" || section.Skill == "READING")
        {
            foreach (var answer in answers)
            {
                if (answer.IsCorrect.HasValue && answer.IsCorrect.Value)
                {
                    rawScore += answer.EarnedScore ?? 0;
                }
            }
            sectionAttempt.RawScore = rawScore;
        }
        else
        {
            // For Writing/Speaking, raw score will be calculated later by AI evaluation
            sectionAttempt.RawScore = null;
        }

        // Update section attempt status
        sectionAttempt.Status = "SUBMITTED";
        sectionAttempt.FinishedAt = DateTime.UtcNow;

        // Calculate scaled band (if applicable) for objective sections
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

        // Save changes
        await _db.SaveChangesAsync(ct);

        return new SubmitSectionResponse(sectionAttempt.Id, sectionAttempt.RawScore, sectionAttempt.ScaledBand, sectionAttempt.Status);
    }

    public async Task<SubmitSpeakingResponse> SubmitSpeakingAsync(long attemptId, long sectionId, SubmitSpeakingRequest request, long userId, CancellationToken ct)
    {
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
            LanguageCode = "en" // Có thể thay đổi dựa trên cấu hình
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

        // Lưu thay đổi
        await _db.SaveChangesAsync(ct);

        // TODO: Gọi AI để đánh giá (ASR, pronunciation, etc.) nếu cần
        // Ví dụ: var evaluation = await _aiService.EvaluateSpeakingAsync(speakingSubmission, ct);

        return new SubmitSpeakingResponse(
            speakingSubmission.Id,
            sectionAttempt.Id,
            asset.Id,
            sectionAttempt.Status
        );
    }

    public async Task<SubmitWritingResponse> SubmitWritingAsync(long attemptId, long sectionId, SubmitWritingRequest request, long userId, CancellationToken ct)
    {
        _logger.LogInformation("Submitting writing for attemptId: {AttemptId}, sectionId: {SectionId}, userId: {UserId}", attemptId, sectionId, userId);

        // Validate request
        if (request == null || string.IsNullOrWhiteSpace(request.ContentText) || !request.ConfirmSubmission)
            throw new InvalidOperationException("Text content and submission confirmation are required.");

        // Kiểm tra TestAttempt
        var testAttempt = await _db.TestAttempts
            .FirstOrDefaultAsync(ta => ta.Id == attemptId && ta.UserId == userId && ta.Status != "CANCELLED", ct);
        if (testAttempt == null)
            throw new InvalidOperationException("Test attempt not found or not accessible.");
        _logger.LogInformation("TestAttempt found: {TestAttemptId}", testAttempt.Id);

        // Kiểm tra SectionAttempt
        var sectionAttempt = await _db.SectionAttempts
            .FirstOrDefaultAsync(sa => sa.TestAttemptId == attemptId && sa.SectionId == sectionId && sa.Status != "CANCELLED", ct);
        if (sectionAttempt == null)
            throw new InvalidOperationException("Section attempt not found or not accessible.");
        _logger.LogInformation("SectionAttempt found: {SectionAttemptId}", sectionAttempt.Id);

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
                Kind = fileExtension == ".pdf" ? "DOC" : "IMAGE", // Sử dụng DOC cho PDF, IMAGE cho JPG/PNG
                StorageUrl = storageUrl,
                MediaType = request.File.ContentType,
                ByteSize = request.File.Length,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                // Thêm các trường khác nếu cần
                Sha256 = null, // Tính SHA256 nếu cần
                LanguageCode = null, // Đặt nếu có thông tin ngôn ngữ
                DurationSec = null // Không áp dụng cho Writing
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

        // Lưu thay đổi
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

    // Helper: Cập nhật UserStats
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

    // Helper: Cập nhật Leaderboard (best attempt cho paper)
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
}