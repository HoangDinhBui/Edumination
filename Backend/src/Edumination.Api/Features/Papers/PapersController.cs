using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Papers.Dtos;
using Edumination.Api.Features.Papers.Services;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using Edumination.Domain.Interfaces;
using Edumination.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Edumination.Api.Papers;

[Route("api/[controller]")]
[ApiController]
public class PapersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context;

    private readonly IPaperService _paperService;

    public PapersController(IUnitOfWork unitOfWork, AppDbContext context, IPaperService paperService)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _paperService = paperService;
    }

    [HttpPost]
public async Task<IActionResult> CreatePaper([FromBody] CreatePaperRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return BadRequest("Tiêu đề là bắt buộc.");
    }

    var userId = GetCurrentUserId();
    var testPaper = new TestPaper
    {
        Title = request.Title,
        UploadMethod = request.UploadMethod,
        PdfAssetId = request.PdfAssetId,
        CreatedBy = userId,
        CreatedAt = DateTime.UtcNow,
        SourceType = "CUSTOM",
        Status = "DRAFT"
    };

    await _unitOfWork.TestPapers.AddAsync(testPaper);

    if (request.UploadMethod == "PDF_PARSER" && request.PdfAssetId.HasValue)
    {
        await ProcessPdfAsync(testPaper.Id, request.PdfAssetId.Value);
    }

    var dto = new TestPaperDto
    {
        Id = testPaper.Id,
        Title = testPaper.Title,
        Status = testPaper.Status,
        CreatedAt = testPaper.CreatedAt,
        PdfAssetId = testPaper.PdfAssetId
    };
    return CreatedAtAction(nameof(GetPaper), new { id = testPaper.Id }, dto);
}
    [HttpGet]
    [Authorize]  // Yêu cầu login, nhưng không giới hạn role
    public async Task<IActionResult> ListPapers([FromQuery] string? status = null)
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        bool isTeacherOrAdmin = roles.Contains("TEACHER") || roles.Contains("ADMIN");

        if (!isTeacherOrAdmin && !string.IsNullOrEmpty(status))
        {
            return Forbid();  // Student không được filter status
        }

        var papers = await _paperService.ListAsync(status, isTeacherOrAdmin);
        return Ok(papers);
    }
    
    [HttpGet("{id}")]
    [Authorize]  // Yêu cầu login, nhưng không giới hạn role
    public async Task<IActionResult> GetPaper(long id)
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        bool isStudent = roles.Contains("STUDENT") && !roles.Contains("TEACHER") && !roles.Contains("ADMIN");

        var paper = await _paperService.GetDetailedAsync(id, hideAnswers: isStudent);
        if (paper == null)
        {
            return NotFound();
        }
        return Ok(paper);
    }

    private async Task ProcessPdfAsync(long paperId, long pdfAssetId)
    {
        var paper = await _unitOfWork.TestPapers.GetByIdAsync(paperId);
        if (paper == null || paper.Status != "DRAFT" || paper.PdfAssetId != pdfAssetId)
        {
            return;
        }

        var section = new TestSection
        {
            PaperId = paperId,
            Skill = "READING",
            SectionNo = 1,
            TimeLimitSec = 0,
            IsPublished = false
        };
        await _unitOfWork.TestSections.AddAsync(section);

        var passage = new Passage
        {
            SectionId = section.Id,
            Title = "Passage 1",
            ContentText = "Nội dung mẫu",
            Position = 1,
            CreatedAt = DateTime.UtcNow
        };
        await _unitOfWork.Passages.AddAsync(passage);

        // Lấy exercise ngẫu nhiên
        var randomExerciseId = await _context.Exercises
            .OrderBy(e => Guid.NewGuid())
            .Select(e => e.Id)
            .FirstAsync();

        // Tìm position lớn nhất hiện có trong exercise này
        var maxPos = await _context.Questions
            .Where(q => q.ExerciseId == randomExerciseId)
            .MaxAsync(q => (int?)q.Position) ?? 0;

        // Tạo question với Position = maxPos + 1
        var question = new Question
        {
            PassageId = passage.Id,
            Qtype = "MCQ",
            Stem = "Câu hỏi mẫu",
            Position = maxPos + 1,
            CreatedAt = DateTime.UtcNow,
            ExerciseId = randomExerciseId
        };
        await _unitOfWork.Questions.AddAsync(question);

        await _unitOfWork.SaveChangesAsync();
    }

private long GetCurrentUserId()
{
    var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid" || c.Type == "sub")?.Value;
    return long.Parse(userIdClaim ?? throw new UnauthorizedAccessException("Không tìm thấy ID người dùng."));
}
}

public class CreatePaperRequest
{
    public string Title { get; set; }
    public string UploadMethod { get; set; } // "PDF_PARSER" or "MANUAL"
    public long? PdfAssetId { get; set; }
}
