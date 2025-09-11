using Edumination.Api.Common.Results;
using Edumination.Api.Domain.Entities.EduDomain;
using Edumination.Api.Features.Admin.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Api.Features.Admin.Services;

public class EduDomainService : IEduDomainService
{
    private readonly AppDbContext _db;
    public EduDomainService(AppDbContext db) => _db = db;

    public async Task<PagedResult<EduDomainItemDto>> GetAsync(EduDomainListQuery query, CancellationToken ct)
    {
        var q = _db.EduDomains.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.q))
            q = q.Where(x => x.Domain.Contains(query.q));

        var total = await q.CountAsync(ct);

        var page = Math.Max(1, query.page);
        var pageSize = Math.Clamp(query.pageSize, 5, 100);

        var items = await q.OrderBy(x => x.Domain)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new EduDomainItemDto
            {
                Id = x.Id,
                Domain = x.Domain
            })
            .ToListAsync(ct);

        return new PagedResult<EduDomainItemDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }

    public async Task<ApiResult<EduDomainItemDto>> CreateAsync(CreateEduDomainRequest req, CancellationToken ct)
    {
        var domain = req.Domain.Trim().ToLowerInvariant();

        var exists = await _db.EduDomains.AnyAsync(x => x.Domain == domain, ct);
        if (exists) return new(false, null, "Domain already exists.");

        var entity = new EduDomain
        {
            Domain = domain
        };
        _db.EduDomains.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new(true, new EduDomainItemDto
        {
            Id = entity.Id,
            Domain = entity.Domain
        }, null);
    }

    public async Task<ApiResult<object>> DeleteAsync(long id, CancellationToken ct)
    {
        var entity = await _db.EduDomains.SingleOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return new(false, null, "Not found.");

        _db.EduDomains.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return new(true, new { }, null);
    }
}
