using Edumination.Api.Features.Attempts.Dtos;
using Edumination.Api.Features.Attempts.Services;
using Edumination.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Edumination.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AttemptsController : ControllerBase
{
    private readonly IAttemptService _attemptService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AttemptsController> _logger;

    public AttemptsController(IAttemptService attemptService, IUnitOfWork unitOfWork, ILogger<AttemptsController> logger)
    {
        _attemptService = attemptService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> StartAttempt([FromBody] StartAttemptRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Received request to start attempt, Request: {@Request}", request);

        if (request == null || request.PaperId <= 0)
        {
            _logger.LogWarning("Invalid request data for starting attempt");
            return BadRequest("Dữ liệu yêu cầu là bắt buộc và phải bao gồm paper_id hợp lệ.");
        }

        var userId = GetCurrentUserId();
        if (userId <= 0)
        {
            _logger.LogWarning("User ID not found in token");
            return Unauthorized("Không tìm thấy ID người dùng.");
        }

        try
        {
            var response = await _attemptService.StartAsync(userId, request, cancellationToken);
            _logger.LogInformation("Attempt started successfully for user ID: {UserId}, attempt ID: {AttemptId}", userId, response.AttemptId);
            return CreatedAtAction(nameof(GetAttempt), new { id = response.AttemptId }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to start attempt for user ID: {UserId}, paper ID: {PaperId}", userId, request.PaperId);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting attempt for user ID: {UserId}, paper ID: {PaperId}", userId, request.PaperId);
            return StatusCode(500, "Có lỗi xảy ra khi khởi tạo attempt.");
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetAttempt(long id)
    {
        var attempt = await _unitOfWork.TestAttempts.GetByIdAsync(id);
        if (attempt == null)
        {
            return NotFound();
        }
        return Ok(new
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            PaperId = attempt.PaperId,
            AttemptNo = attempt.AttemptNo,
            StartedAt = attempt.StartedAt,
            FinishedAt = attempt.FinishedAt,
            Status = attempt.Status
        });
    }

    [HttpPost("{aid}/sections/{sid}/answer")]
    [Authorize]
    public async Task<IActionResult> SubmitAnswer(long aid, long sid, [FromBody] SubmitAnswerRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Received request to submit answer, Attempt ID: {Aid}, Section ID: {Sid}, Request: {@Request}", aid, sid, request);

        if (request == null || request.QuestionId <= 0)
        {
            _logger.LogWarning("Invalid request data for submitting answer");
            return BadRequest("Dữ liệu yêu cầu là bắt buộc và phải bao gồm question_id hợp lệ.");
        }

        var userId = GetCurrentUserId();
        if (userId <= 0)
        {
            _logger.LogWarning("User ID not found in token");
            return Unauthorized("Không tìm thấy ID người dùng.");
        }

        try
        {
            var response = await _attemptService.SubmitAnswerAsync(aid, sid, request, userId, cancellationToken);
            _logger.LogInformation("Answer submitted successfully for attempt ID: {Aid}, section ID: {Sid}, question ID: {QuestionId}", aid, sid, request.QuestionId);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to submit answer for attempt ID: {Aid}, section ID: {Sid}, question ID: {QuestionId}", aid, sid, request.QuestionId);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting answer for attempt ID: {Aid}, section ID: {Sid}, question ID: {QuestionId}", aid, sid, request.QuestionId);
            return StatusCode(500, "Có lỗi xảy ra khi lưu đáp án.");
        }
    }

    [HttpPost("{aid}/sections/{sid}/submit")]
    [Authorize]
    public async Task<IActionResult> SubmitSection(long aid, long sid, [FromBody] SubmitSectionRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Nhận yêu cầu nộp phần thi, Attempt ID: {Aid}, Section ID: {Sid}, Request: {@Request}", aid, sid, request);

        if (request == null)
        {
            _logger.LogWarning("Dữ liệu yêu cầu không hợp lệ để nộp phần thi");
            return BadRequest("Dữ liệu yêu cầu là bắt buộc.");
        }

        var userId = GetCurrentUserId();
        if (userId <= 0)
        {
            _logger.LogWarning("Không tìm thấy ID người dùng trong token");
            return Unauthorized("Không tìm thấy ID người dùng.");
        }

        try
        {
            var response = await _attemptService.SubmitSectionAsync(aid, sid, request, userId, cancellationToken);
            _logger.LogInformation("Phần thi được nộp thành công cho attempt ID: {Aid}, section ID: {Sid}", aid, sid);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Thất bại khi nộp phần thi cho attempt ID: {Aid}, section ID: {Sid}", aid, sid);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi nộp phần thi cho attempt ID: {Aid}, section ID: {Sid}", aid, sid);
            return StatusCode(500, "Có lỗi xảy ra khi gửi phần thi.");
        }
    }

    [HttpPost("{aid}/sections/{sid}/speaking")]
    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = 104857600)] // Giới hạn file 100MB
    [RequestSizeLimit(104857600)] // Giới hạn kích thước request
    public async Task<IActionResult> SubmitSpeaking(long aid, long sid, [FromForm] SubmitSpeakingRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Received request to submit speaking, Attempt ID: {Aid}, Section ID: {Sid}", aid, sid);

        if (request == null || request.AudioFile == null)
        {
            _logger.LogWarning("Invalid request data for submitting speaking");
            return BadRequest("Audio file is required.");
        }

        var userId = GetCurrentUserId();
        if (userId <= 0)
        {
            _logger.LogWarning("User ID not found in token");
            return Unauthorized("Không tìm thấy ID người dùng.");
        }

        try
        {
            var response = await _attemptService.SubmitSpeakingAsync(aid, sid, request, userId, cancellationToken);
            _logger.LogInformation("Speaking submitted successfully for attempt ID: {Aid}, section ID: {Sid}, submission ID: {SubmissionId}", aid, sid, response.SubmissionId);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to submit speaking for attempt ID: {Aid}, section ID: {Sid}", aid, sid);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting speaking for attempt ID: {Aid}, section ID: {Sid}", aid, sid);
            return StatusCode(500, "Có lỗi xảy ra khi nộp bài thi Speaking.");
        }
    }

    [HttpPost("{aid}/sections/{sid}/writing")]
    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = 104857600)] // Giới hạn file 100MB
    [RequestSizeLimit(104857600)] // Giới hạn kích thước request
    public async Task<IActionResult> SubmitWriting(long aid, long sid, [FromForm] SubmitWritingRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Received request to submit writing, Attempt ID: {Aid}, Section ID: {Sid}", aid, sid);

        if (request == null || string.IsNullOrWhiteSpace(request.ContentText))
        {
            _logger.LogWarning("Invalid request data for submitting writing");
            return BadRequest("Text content is required.");
        }

        var userId = GetCurrentUserId();
        if (userId <= 0)
        {
            _logger.LogWarning("User ID not found in token");
            return Unauthorized("Không tìm thấy ID người dùng.");
        }

        try
        {
            var response = await _attemptService.SubmitWritingAsync(aid, sid, request, userId, cancellationToken);
            _logger.LogInformation("Writing submitted successfully for attempt ID: {Aid}, section ID: {Sid}, submission ID: {SubmissionId}", aid, sid, response.SubmissionId);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to submit writing for attempt ID: {Aid}, section ID: {Sid}", aid, sid);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting writing for attempt ID: {Aid}, section ID: {Sid}", aid, sid);
            return StatusCode(500, "Có lỗi xảy ra khi nộp bài thi Writing.");
        }
    }

    [HttpPost("{aid}/submit")]
    [Authorize]
    public async Task<IActionResult> SubmitTest(long aid, [FromBody] SubmitTestRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Received request to submit test attempt ID: {Aid}", aid);

        if (request == null || !request.ConfirmSubmission)
        {
            _logger.LogWarning("Invalid request data for submitting test");
            return BadRequest("Submission confirmation is required.");
        }

        var userId = GetCurrentUserId();
        if (userId <= 0)
        {
            _logger.LogWarning("User ID not found in token");
            return Unauthorized("Không tìm thấy ID người dùng.");
        }

        try
        {
            var response = await _attemptService.SubmitTestAsync(aid, request, userId, cancellationToken);
            _logger.LogInformation("Test submitted successfully for attempt ID: {Aid}, overall band: {OverallBand}", aid, response.OverallBand);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to submit test for attempt ID: {Aid}", aid);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting test for attempt ID: {Aid}", aid);
            return StatusCode(500, "Có lỗi xảy ra khi nộp bài thi.");
        }
    }

    [HttpGet("{aid}/sections/{sid}/result")]
    [Authorize]
    public async Task<IActionResult> GetResult(long aid, long sid, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _attemptService.GetSectionResultAsync(aid, sid, userId, ct);
        return Ok(result);
    }

    // GetCurrentUserId giữ nguyên

    private long GetCurrentUserId()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid" || c.Type == "sub")?.Value;
        return long.Parse(userIdClaim ?? throw new UnauthorizedAccessException("Không tìm thấy ID người dùng."));
    }
}