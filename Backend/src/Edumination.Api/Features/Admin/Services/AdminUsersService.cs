using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Features.Admin.Dtos;
using Edumination.Api.Common.Results;
using Edumination.Api.Common.Services;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Features.Admin.Services;

public interface IAdminUsersService
{
    Task<PagedResult<AdminUserItemDto>> GetUsersAsync(AdminUserQuery q, CancellationToken ct);
    Task<ApiResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest req, CancellationToken ct);
    Task<ApiResult<AdminUserDto>> PatchUserAsync(long id, PatchUserRequest req, CancellationToken ct);
}

public class AdminUsersService : IAdminUsersService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    public AdminUsersService(AppDbContext db, IPasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

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

    public async Task<ApiResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest req, CancellationToken ct)
    {
        var emailNorm = req.Email.Trim().ToLowerInvariant();

        // Check trùng email
        if (await _db.Users.AnyAsync(u => u.Email == emailNorm, ct))
            return new(false, null, "Email already exists.");

        // Tạo user
        var user = new User
        {
            Email = emailNorm,
            FullName = req.FullName.Trim(),
            PasswordHash = _hasher.Hash(req.Password),
            IsActive = req.Active,
            EmailVerified = true
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        // Gán role
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Code == req.Role, ct);
        if (role != null)
        {
            _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
            await _db.SaveChangesAsync(ct);
        }

        return new(true, new CreateUserResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = req.Role
        }, null);
    }

    public async Task<ApiResult<AdminUserDto>> PatchUserAsync(long id, PatchUserRequest req, CancellationToken ct)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id, ct);
        if (user is null)
            return new(false, null, "User not found.");

        // Email
        if (!string.IsNullOrWhiteSpace(req.Email))
        {
            var emailNorm = req.Email.Trim().ToLowerInvariant();
            var dup = await _db.Users.AnyAsync(u => u.Email == emailNorm && u.Id != id, ct);
            if (dup) return new(false, null, "Email already in use.");

            user.Email = emailNorm;
        }

        // FullName
        if (!string.IsNullOrWhiteSpace(req.FullName))
            user.FullName = req.FullName.Trim();

        // Active
        if (req.Active.HasValue)
            user.IsActive = req.Active.Value;

        // Password
        if (!string.IsNullOrWhiteSpace(req.Password))
            user.PasswordHash = _hasher.Hash(req.Password);

        // Role (đơn — nếu bạn dùng nhiều role, sửa logic bên dưới để add/remove thay vì “replace”)
        if (!string.IsNullOrWhiteSpace(req.Role))
        {
            var code = req.Role.Trim().ToUpperInvariant();
            var role = await _db.Roles.SingleOrDefaultAsync(r => r.Code == code, ct);
            if (role is null)
                return new(false, null, $"Role '{code}' not found.");

            // Xóa toàn bộ role cũ (nếu mô hình 1 user = 1 role)
            var existingUserRoles = await _db.UserRoles.Where(ur => ur.UserId == id).ToListAsync(ct);
            if (existingUserRoles.Count > 0)
                _db.UserRoles.RemoveRange(existingUserRoles);

            _db.UserRoles.Add(new UserRole { UserId = id, RoleId = role.Id });
        }

        await _db.SaveChangesAsync(ct);

        // build DTO trả về
        var roles = await _db.UserRoles
            .Where(ur => ur.UserId == id)
            .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Code)
            .ToArrayAsync(ct);

        var dto = new AdminUserDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            IsActive = user.IsActive,
            Roles = roles
        };

        return new(true, dto, null);
    }
}
