using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Papers.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using Edumination.Domain.Interfaces;
using Edumination.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Edumination.Api.Papers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "TEACHER,ADMIN")]
public class PapersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context;

    public PapersController(IUnitOfWork unitOfWork, AppDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaper(long id)
    {
        var paper = await _unitOfWork.TestPapers.GetByIdAsync(id);
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
    var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id" || c.Type == "sub")?.Value;
    return long.Parse(userIdClaim ?? throw new UnauthorizedAccessException("Không tìm thấy ID người dùng."));
}
}

public class CreatePaperRequest
{
    public string Title { get; set; }
    public string UploadMethod { get; set; } // "PDF_PARSER" or "MANUAL"
    public long? PdfAssetId { get; set; }
}