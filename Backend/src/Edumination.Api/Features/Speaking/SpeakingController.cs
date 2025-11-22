using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using Edumination.Api.Features.Passage.Dtos;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Edumination.Api.Features.Application;

[ApiController]
[Route("api/v1/attempts/{attemptId}/sections/{sectionId}/speaking")]
[Authorize]
public class SpeakingController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAssetService _assetService;
    private readonly ILogger<SpeakingController> _logger;

    // ISpeakingGradingService và ISectionAttemptRepository
    public SpeakingController(
        AppDbContext context,
        IAssetService assetService,
        ILogger<SpeakingController> logger)
    {
        _context = context;
        _assetService = assetService;
        _logger = logger;
    }

    private long GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitSpeaking(
        long attemptId,
        long sectionId,
        [FromForm] SubmitSpeakingRequest request)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized("User not authenticated");

        var sectionAttempt = await _context.SectionAttempts
            .Include(sa => sa.TestAttempt)
            .FirstOrDefaultAsync(sa => 
                sa.Id == sectionId && 
                sa.TestAttempt!.UserId == userId
            );

        if (sectionAttempt == null)
            return NotFound("Section attempt not found");

        if (sectionAttempt.Status != "IN_PROGRESS")
            return BadRequest("Section is not in progress");

        try
        {
            Asset audioAsset;
            using (var stream = request.AudioFile.OpenReadStream())
            {
                audioAsset = await _assetService.UploadAsync(
                    stream,
                    request.AudioFile.FileName,
                    request.AudioFile.ContentType,
                    userId,
                    AssetKind.AUDIO
                );
            }

            var submission = new SpeakingSubmission
            {
                SectionAttemptId = sectionId,
                QuestionId = request.QuestionId,
                PromptText = request.PromptText,
                AudioAssetId = audioAsset.Id,
                AudioUrl = $"/api/v1/assets/download/{audioAsset.Id}",
                Status = "PENDING",
                CreatedAt = DateTime.UtcNow,
                AIProvider = "Groq"
            };

            _context.SpeakingSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Created speaking submission {SubmissionId} for section {SectionId}", 
                submission.Id, 
                sectionId
            );

            return Accepted(new
            {
                SubmissionId = submission.Id,
                Status = "PENDING",
                Message = "Submission received. Grading in progress..."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting speaking recording");
            return StatusCode(500, new 
            { 
                Title = "Submission Failed",
                Detail = ex.Message 
            });
        }
    }

    [HttpGet("{submissionId}")]
    public async Task<IActionResult> GetSubmissionResult(
        long attemptId,
        long sectionId,
        long submissionId)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var submission = await _context.SpeakingSubmissions
            .Include(s => s.SectionAttempt)
                .ThenInclude(sa => sa!.TestAttempt)
            .Include(s => s.AudioAsset)
            .FirstOrDefaultAsync(s => 
                s.Id == submissionId && 
                s.SectionAttempt!.TestAttempt!.UserId == userId
            );

        if (submission == null)
            return NotFound("Submission not found");

        var response = new SpeakingSubmissionResponse
        {
            SubmissionId = submission.Id,
            SectionAttemptId = submission.SectionAttemptId,
            QuestionId = submission.QuestionId,
            PromptText = submission.PromptText,
            AudioUrl = submission.AudioUrl,
            DurationSec = submission.DurationSec,
            TranscribedText = submission.TranscribedText,
            WordsCount = submission.WordsCount,
            OverallScore = submission.OverallScore.HasValue ? (decimal?)submission.OverallScore.Value : null,
            BandScore = submission.OverallScore.HasValue ? (decimal?)submission.OverallScore.Value : null,
            FluencyScore = submission.FluencyScore.HasValue ? (decimal?)submission.FluencyScore.Value : null,
            GrammarScore = submission.GrammarScore.HasValue ? (decimal?)submission.GrammarScore.Value : null,
            VocabularyScore = submission.VocabularyScore.HasValue ? (decimal?)submission.VocabularyScore.Value : null,
            PronunciationScore = submission.PronunciationScore.HasValue ? (decimal?)submission.PronunciationScore.Value : null,
            FeedbackSummary = submission.AIFeedback,
            Status = submission.Status,
            AIModel = submission.AIModel,
            CreatedAt = submission.CreatedAt,
            GradedAt = submission.GradedAt
        };

        return Ok(response);
    }
}

public class SubmitSpeakingRequest
{
    public IFormFile AudioFile { get; set; } = null!;
    public string PromptText { get; set; } = string.Empty;
    public long? QuestionId { get; set; }
}

public class SpeakingSubmissionResponse
{
    public long SubmissionId { get; set; }
    public long SectionAttemptId { get; set; }
    public long? QuestionId { get; set; }
    public string? PromptText { get; set; }
    public string? AudioUrl { get; set; }
    public int? DurationSec { get; set; }
    public string? TranscribedText { get; set; }
    public int? WordsCount { get; set; }
    public decimal? OverallScore { get; set; }
    public decimal? BandScore { get; set; }
    public decimal? FluencyScore { get; set; }
    public decimal? GrammarScore { get; set; }
    public decimal? VocabularyScore { get; set; }
    public decimal? PronunciationScore { get; set; }
    public string? FeedbackSummary { get; set; }
    public string Status { get; set; } = "PENDING";
    public string? AIModel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? GradedAt { get; set; }
}