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

    public async Task<IReadOnlyList<PaperListItemDto>> ListAsync(string? statusFilter, bool isTeacherOrAdmin, CancellationToken ct = default)
    {
        var query = _db.TestPapers.AsNoTracking();

        if (!isTeacherOrAdmin)
        {
            // Student chỉ thấy PUBLISHED
            query = query.Where(p => p.Status == "PUBLISHED");
        }
        else if (!string.IsNullOrEmpty(statusFilter))
        {
            // Teacher/Admin: filter by status (e.g., "DRAFT|REVIEW" -> split and OR)
            var statuses = statusFilter.Split('|').Select(s => s.Trim().ToUpper()).ToArray();
            query = query.Where(p => statuses.Contains(p.Status.ToUpper()));
        }
        // Else: all for Teacher/Admin

        return await query
            .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
            .Select(p => new PaperListItemDto(p.Id, p.Title, p.Status, p.CreatedAt))
            .ToListAsync(ct);
    }

    public async Task<DetailedPaperDto?> GetDetailedAsync(long id, bool hideAnswers, CancellationToken ct = default)
{
    var paper = await _db.TestPapers
        .AsNoTracking()
        .Include(p => p.TestSections)
            .ThenInclude(s => s.Passages)
                .ThenInclude(pa => pa.Questions)
                    .ThenInclude(q => q.QuestionChoices)
                    .ThenInclude(q => q.QuestionAnswerKey)
        .FirstOrDefaultAsync(p => p.Id == id, ct);

    if (paper == null) return null;

    var dto = new DetailedPaperDto
    {
        Id = paper.Id,
        Title = paper.Title,
        Status = paper.Status,
        CreatedAt = paper.CreatedAt,
        PdfAssetId = paper.PdfAssetId,
        Sections = paper.TestSections.Select(s => new SectionDto
        {
            Id = s.Id,
            Skill = s.Skill,
            SectionNo = s.SectionNo,
            Passages = s.Passages.Select(pa => new PassageDto
            {
                Id = pa.Id,
                Title = pa.Title,
                ContentText = pa.ContentText,
                Questions = pa.Questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Qtype = q.Qtype,
                    Stem = q.Stem,
                    Position = q.Position,
                    Choices = hideAnswers ? null : q.QuestionChoices?.Select(c => new ChoiceDto { Content = c.Content, IsCorrect = c.IsCorrect }).ToList(),
                    AnswerKey = hideAnswers ? null : q.QuestionAnswerKey?.KeyJson // Sử dụng q.QuestionAnswerKey, không phải q.QuestionChoices
                }).ToList()
            }).ToList()
        }).ToList()
    };

    return dto;
}
}