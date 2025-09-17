using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Attempts.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using System.Text.Json;

namespace Edumination.Api.Features.Attempts.Services;

public class AttemptService : IAttemptService
{
    private readonly AppDbContext _db;

    public AttemptService(AppDbContext db) => _db = db;

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
}