using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Papers.Dtos;
using Edumination.Api.Features.Papers.Services;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using Edumination.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Edumination.Api.Papers;

[Route("api/v1/[controller]")]
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

    [HttpGet("{id}/sections")]
    [Authorize] // Yêu cầu đăng nhập
    public async Task<IActionResult> GetPaperSections(long id)
    {
        // Kiểm tra vai trò của người dùng
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        bool isStudent = roles.Contains("STUDENT") && !roles.Contains("TEACHER") && !roles.Contains("ADMIN");
        bool hideAnswers = isStudent;

        // Lấy TestPaper và các section liên quan
        var paper = await _unitOfWork.TestPapers.GetByIdAsync(id);
        if (paper == null)
        {
            return NotFound($"Bài kiểm tra với ID {id} không tồn tại.");
        }

        var sections = await _context.TestSections
            .AsNoTracking()
            .Include(s => s.Passages)
                .ThenInclude(p => p.Questions)
                    .ThenInclude(q => q.QuestionChoices)
            .Include(s => s.Passages)
                .ThenInclude(p => p.Questions)
                    .ThenInclude(q => q.QuestionAnswerKey)
            .Where(s => s.PaperId == id)
            .ToListAsync();

        if (sections == null || !sections.Any())
        {
            return NotFound($"Không tìm thấy section nào cho bài kiểm tra với ID {id}.");
        }

        // Chuyển đổi sang DTO
        var sectionDtos = sections.Select(s => new SectionDto
        {
            Id = s.Id,
            Skill = s.Skill,
            SectionNo = s.SectionNo,
            TimeLimitSec = s.TimeLimitSec ?? 0,
            AudioAssetId = s.AudioAssetId,
            IsPublished = s.IsPublished,
            Passages = s.Passages.Select(p => new PassageDto
            {
                Id = p.Id,
                Title = p.Title ?? string.Empty,
                ContentText = p.ContentText ?? string.Empty,
                Questions = p.Questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Qtype = q.Qtype ?? string.Empty,
                    Stem = q.Stem ?? string.Empty,
                    Position = q.Position,
                    Choices = hideAnswers ? null : q.QuestionChoices?.Select(c => new ChoiceDto
                    {
                        Content = c.Content ?? string.Empty,
                        IsCorrect = c.IsCorrect
                    }).ToList(),
                    AnswerKey = hideAnswers ? null : q.QuestionAnswerKey?.KeyJson
                }).ToList()
            }).ToList()
        }).ToList();

        return Ok(sectionDtos);
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
            UploadMethod = request.UploadMethod ?? "MANUAL",
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
    [Authorize] // Yêu cầu login, nhưng không giới hạn role
    public async Task<IActionResult> ListPapers([FromQuery] string? status = null)
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        bool isTeacherOrAdmin = roles.Contains("TEACHER") || roles.Contains("ADMIN");

        if (!isTeacherOrAdmin && !string.IsNullOrEmpty(status))
        {
            return Forbid(); // Student không được filter status
        }

        var papers = await _paperService.ListAsync(status, isTeacherOrAdmin);
        return Ok(papers);
    }

    [HttpGet("{id}")]
    [Authorize] // Yêu cầu login, nhưng không giới hạn role
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

    [HttpPost("{id}/sections")]
    [Authorize(Roles = "TEACHER,ADMIN")]
    public async Task<IActionResult> CreateSection(long id, [FromBody] SectionCreateRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Skill))
        {
            return BadRequest("Dữ liệu section là bắt buộc và phải bao gồm skill.");
        }

        var paper = await _unitOfWork.TestPapers.GetByIdAsync(id);
        if (paper == null)
        {
            return NotFound($"Bài kiểm tra với ID {id} không tồn tại.");
        }

        var existingSection = await _unitOfWork.TestSections
            .GetQueryable()
            .FirstOrDefaultAsync(s => s.PaperId == id && s.Skill == request.Skill);

        if (request.AudioAssetId.HasValue)
        {
            var audioAsset = await _unitOfWork.Assets.GetByIdAsync(request.AudioAssetId.Value);
            if (audioAsset == null || audioAsset.Kind != "AUDIO")
            {
                return BadRequest("Audio asset ID không hợp lệ hoặc không phải là file audio.");
            }
        }

        var section = existingSection ?? new TestSection
        {
            PaperId = id,
            Skill = request.Skill,
            SectionNo = request.SectionNo,
            TimeLimitSec = request.TimeLimitSec,
            AudioAssetId = request.AudioAssetId,
            IsPublished = false
        };

        if (existingSection != null)
        {
            section.SectionNo = request.SectionNo;
            section.TimeLimitSec = request.TimeLimitSec;
            section.AudioAssetId = request.AudioAssetId;
            _context.TestSections.Update(section); // Cập nhật thay vì tạo mới
        }
        else
        {
            await _unitOfWork.TestSections.AddAsync(section);
        }

        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSection), new { id = section.Id }, new
        {
            Id = section.Id,
            PaperId = section.PaperId,
            Skill = section.Skill,
            SectionNo = section.SectionNo,
            TimeLimitSec = section.TimeLimitSec,
            AudioAssetId = section.AudioAssetId,
            IsPublished = section.IsPublished
        });
    }

    [HttpGet("sections/{id}")]
    [Authorize]
    public async Task<IActionResult> GetSection(long id)
    {
        var section = await _unitOfWork.TestSections.GetByIdAsync(id);
        if (section == null)
        {
            return NotFound();
        }
        return Ok(new
        {
            Id = section.Id,
            PaperId = section.PaperId,
            Skill = section.Skill,
            SectionNo = section.SectionNo,
            TimeLimitSec = section.TimeLimitSec,
            AudioAssetId = section.AudioAssetId,
            IsPublished = section.IsPublished
        });
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

        var randomExerciseId = await _context.Exercises
            .OrderBy(e => Guid.NewGuid())
            .Select(e => e.Id)
            .FirstAsync();

        var maxPos = await _context.Questions
            .Where(q => q.ExerciseId == randomExerciseId)
            .MaxAsync(q => (int?)q.Position) ?? 0;

        var question = new Question
        {
            PassageId = passage.Id,
            Qtype = "MCQ",
            Stem = "Câu hỏi mẫu",
            Position = maxPos + 1,
            CreatedAt = DateTime.UtcNow,
            ExerciseId = randomExerciseId
        };
        _ = await _unitOfWork.Questions.CreateAsync(question);

        await _unitOfWork.SaveChangesAsync(); // Lưu tất cả cùng lúc
        Console.WriteLine($"Section ID: {section.Id}, Passage ID: {passage.Id}, Question Passage ID: {question.PassageId}");
    }
    private long GetCurrentUserId()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid" || c.Type == "sub")?.Value;
        return long.Parse(userIdClaim ?? throw new UnauthorizedAccessException("Không tìm thấy ID người dùng."));
    }
}

public class CreatePaperRequest
{
    public string? Title { get; set; }
    public string? UploadMethod { get; set; } // "PDF_PARSER" or "MANUAL"
    public long? PdfAssetId { get; set; }
}

public class SectionCreateRequest
{
    public string? Skill { get; set; } // ENUM: LISTENING, READING, WRITING, SPEAKING
    public int SectionNo { get; set; }
    public int TimeLimitSec { get; set; }
    public long? AudioAssetId { get; set; }
}