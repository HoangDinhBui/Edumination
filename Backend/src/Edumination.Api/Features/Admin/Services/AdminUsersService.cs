using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Features.Admin.Dtos;

namespace Edumination.Api.Features.Admin.Services;

public interface IAdminUsersService
{
    Task<PagedResult<AdminUserItemDto>> GetUsersAsync(AdminUserQuery q, CancellationToken ct);
}

public class AdminUsersService : IAdminUsersService
{
    private readonly AppDbContext _db;
    public AdminUsersService(AppDbContext db) { _db = db; }

    public async Task<PagedResult<AdminUserItemDto>> GetUsersAsync(AdminUserQuery q, CancellationToken ct)
    {
        var users = _db.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Email))
        {
            var email = q.Email.Trim().ToLower();
            users = users.Where(u => u.Email.Contains(email));
        }

        if (q.Active.HasValue)
        {
            users = users.Where(u => u.IsActive == q.Active.Value);
        }

        IQueryable<long> userIdsFilterByRole = null!;
        if (!string.IsNullOrWhiteSpace(q.Role))
        {
            var roleCode = q.Role.Trim().ToUpper();
            userIdsFilterByRole =
                _db.UserRoles.AsNoTracking()
                  .Join(_db.Roles.AsNoTracking(),
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => new { ur.UserId, r.Code })
                  .Where(x => x.Code == roleCode)
                  .Select(x => x.UserId)
                  .Distinct();

            users = users.Where(u => userIdsFilterByRole.Contains(u.Id));
        }

        var total = await users.LongCountAsync(ct);

        var page = q.Page <= 0 ? 1 : q.Page;
        var pageSize = q.PageSize <= 0 ? 20 : Math.Min(q.PageSize, 100);

        var pageUserIds = await users
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => u.Id)
            .ToListAsync(ct);

        var rolesByUser = await _db.UserRoles.AsNoTracking()
            .Where(ur => pageUserIds.Contains(ur.UserId))
            .Join(_db.Roles.AsNoTracking(),
                  ur => ur.RoleId,
                  r => r.Id,
                  (ur, r) => new { ur.UserId, r.Code })
            .GroupBy(x => x.UserId)
            .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.Code).ToArray(), ct);

        var items = await _db.Users.AsNoTracking()
            .Where(u => pageUserIds.Contains(u.Id))
            .Select(u => new AdminUserItemDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                IsActive = u.IsActive,
                EmailVerified = u.EmailVerified,
                CreatedAt = u.CreatedAt,
                Roles = Array.Empty<string>()
            })
            .ToListAsync(ct);

        foreach (var it in items)
            it.Roles = rolesByUser.TryGetValue(it.Id, out var arr) ? arr : Array.Empty<string>();

        items = items.OrderByDescending(i => i.CreatedAt).ToList();

        return new PagedResult<AdminUserItemDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }
}
