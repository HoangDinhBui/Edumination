using Edumination.Api.Features.Papers.Dtos;
using Edumination.Api.Features.Papers.Services;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Edumination.Api.Features.Papers.Services;

public class PaperService : IPaperService
{
    private readonly AppDbContext _db;

    public PaperService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<PaperListItemDto>> ListPublishedAsync(CancellationToken ct)
    {
        // Giữ nguyên
        return await _db.TestPapers.AsNoTracking()
            .Where(p => p.Status == "PUBLISHED")
            .OrderByDescending(p => p.PublishedAt)
            .Select(p => new PaperListItemDto(p.Id, p.Title, p.Status, p.CreatedAt))
            .ToListAsync(ct);
    }

    public async Task<PaperLibraryResponseDto> ListAsync(
        string? status,
        string? skill,
        string? search,
        string? sort,
        bool isTeacherOrAdmin,
        CancellationToken ct = default)
    {
        string responseTitle;
        var items = new List<PaperLibraryItemDto>();

        // Chuẩn hóa skill về chữ in hoa, nếu nó null thì mặc định là "ALL SKILLS"
        string normalizedSkill = string.IsNullOrEmpty(skill) ? "ALL SKILLS" : skill.ToUpper();

        // Xử lý logic lọc
        switch (normalizedSkill)
        {
            case "ALL SKILLS":
                // Logic này khớp với mock data "All Skills" của bạn
                responseTitle = "IELTS Mock Test 2025";

                // Truy vấn bảng MockTestQuarters (giả sử bạn có _db.MockTestQuarters)
                // LƯU Ý: Tính "tests_taken" cho Quarter rất phức tạp.
                // Tạm thời, chúng ta sẽ trả về 0 hoặc một giá trị giả.
                // Để có số thật, bạn cần SUM(attempts) từ 4 paper_id (L, R, W, S)
                items = await _db.MockTestQuarters
                    .AsNoTracking()
                    .Where(q => q.Status == "PUBLISHED") // Chỉ lấy các quý đã xuất bản
                    .Select(q => new PaperLibraryItemDto
                    {
                        Id = q.Id,
                        Name = $"Quarter {q.Quarter} - Set {q.SetNumber}",
                        Taken = 0 // Tạm thời
                    })
                    .ToListAsync(ct);
                break;

            case "LISTENING":
            case "READING":
            case "WRITING":
            case "SPEAKING":
                // Logic này khớp với các mock data còn lại
                responseTitle = $"IELTS {skill.ToTitleCase()} Tests";

                var query = _db.TestPapers.AsNoTracking()
                                // Chỉ lấy paper CÓ section tương ứng với skill
                                .Where(p => p.TestSections.Any(s => s.Skill == normalizedSkill));

                // Lọc theo Status (PUBLISHED cho student, hoặc theo filter cho admin)
                if (!isTeacherOrAdmin)
                {
                    query = query.Where(p => p.Status == "PUBLISHED");
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status == status);
                }

                // Lọc theo Search
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Title.Contains(search));
                }

                // Sắp xếp
                if (sort == "latest")
                {
                    query = query.OrderByDescending(p => p.PublishedAt ?? p.CreatedAt);
                }
                // (Bạn có thể thêm các logic sort khác ở đây)

                // Chiếu (Project) sang DTO
                items = await query
                    .Select(p => new PaperLibraryItemDto
                    {
                        Id = p.Id,
                        Name = p.Title,
                        // Đếm số lượt làm bài từ bảng TestAttempts
                        Taken = p.TestAttempts.Count()
                    })
                    .ToListAsync(ct);
                break;

            default:
                responseTitle = "Unknown Skill";
                items = new List<PaperLibraryItemDto>();
                break;
        }

        return new PaperLibraryResponseDto
        {
            Title = responseTitle,
            Items = items
        };
    }

    public async Task<DetailedPaperDto?> GetDetailedAsync(long id, bool hideAnswers, CancellationToken ct = default)
{
    var paper = await _db.TestPapers
        .AsNoTracking()
        .AsSplitQuery() // <-- GIỮ LẠI DÒNG QUAN TRỌNG NÀY
        .Include(p => p.TestSections)
            .ThenInclude(s => s.Passages)
                .ThenInclude(pa => pa.Questions)
                    .ThenInclude(q => q.QuestionChoices) // Tải QuestionChoices
        .Include(p => p.TestSections)
            .ThenInclude(s => s.Passages)
                .ThenInclude(pa => pa.Questions)
                    .ThenInclude(q => q.QuestionAnswerKey) // Tải QuestionAnswerKey riêng
        .FirstOrDefaultAsync(p => p.Id == id, ct);

    if (paper == null) return null;

    var dto = new DetailedPaperDto
    {
        Id = paper.Id,
        Title = paper.Title,
        Status = paper.Status,
        CreatedAt = paper.CreatedAt,
        PdfAssetId = paper.PdfAssetId,
        // (Đã xóa PdfStorageUrl)
        Sections = paper.TestSections.Select(s => new SectionDto
        {
            Id = s.Id,
            Skill = s.Skill,
            SectionNo = s.SectionNo,
            TimeLimitSec = s.TimeLimitSec ?? 0,
            AudioAssetId = s.AudioAssetId,
            // (Đã xóa AudioStorageUrl)
            Passages = s.Passages.Select(pa => new PassageDto
            {
                Id = pa.Id,
                Title = pa.Title ?? string.Empty,
                ContentText = pa.ContentText ?? string.Empty,
                Questions = pa.Questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Qtype = q.Qtype ?? string.Empty,
                    Stem = q.Stem ?? string.Empty,
                    Position = q.Position,
                    Choices = hideAnswers ? null : (q.QuestionChoices?.Select(c => new ChoiceDto { Content = c.Content ?? string.Empty, IsCorrect = c.IsCorrect }).ToList() ?? new List<ChoiceDto>()),
                    AnswerKey = hideAnswers ? null : q.QuestionAnswerKey?.KeyJson ?? string.Empty
                }).ToList()
            }).ToList()
        }).ToList()
    };

    return dto;
}
}
public static class StringExtensions
    {
        public static string ToTitleCase(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }
    }