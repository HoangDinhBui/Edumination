namespace Edumination.Api.Features.Papers.Services;

using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Features.Papers.Dtos;

public class PaperService : IPaperService
{
    private readonly AppDbContext _db;
    public PaperService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<PaperListItemDto>> ListPublishedAsync(CancellationToken ct)
        => await _db.TestPapers.AsNoTracking()
            .Where(p => p.Status == "PUBLISHED")
            .OrderByDescending(p => p.PublishedAt)
            .Select(p => new PaperListItemDto(p.Id, p.Title, p.Status, p.CreatedAt))
            .ToListAsync(ct);
}
