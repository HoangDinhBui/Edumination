using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Attempts.Dtos;
using Edumination.Api.Infrastructure.Persistence;

namespace Edumination.Api.Features.Attempts.Services;

public class AttemptService : IAttemptService
{
    private readonly AppDbContext _db;

    public AttemptService(AppDbContext db) => _db = db;

    public async Task<StartAttemptResponse> StartAsync(long userId, StartAttemptRequest req, CancellationToken ct)
    {
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
}