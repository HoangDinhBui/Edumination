using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Features.Courses.Services;

public interface ICourseService
{
    Task<PagedResult<CourseItemDto>> GetAsync(CourseListQuery query, ClaimsPrincipal? user, CancellationToken ct);
    Task<bool> EnrollAsync(long courseId, long userId, CancellationToken ct);
    Task<bool> UnenrollAsync(long courseId, long userId, CancellationToken ct);
}

public class CourseService : ICourseService
{
    private readonly AppDbContext _db;
    public CourseService(AppDbContext db) => _db = db;

    public async Task<PagedResult<CourseItemDto>> GetAsync(CourseListQuery query, ClaimsPrincipal? user, CancellationToken ct)
    {
        var q = _db.Courses.AsQueryable();

        if (query.published.HasValue)
            q = q.Where(c => c.IsPublished == query.published.Value);

        if (!string.IsNullOrWhiteSpace(query.q))
            q = q.Where(c => c.Title.Contains(query.q));

        if (!string.IsNullOrWhiteSpace(query.level) &&
            Enum.TryParse<CourseLevel>(query.level, true, out var lvl))
        {
            q = q.Where(c => c.Level == lvl);
        }

        var total = await q.CountAsync(ct);

        var page = Math.Max(1, query.page);
        var pageSize = Math.Clamp(query.pageSize, 5, 100);

        // userId để join enrollments (nếu có token)
        long? currentUserId = null;
        if (user != null)
        {
            var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            if (long.TryParse(idStr, out var uid)) currentUserId = uid;
        }

        var itemsQuery = q
            .OrderByDescending(c => c.UpdatedAt)
            .ThenBy(c => c.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        if (currentUserId is null)
        {
            var items = await itemsQuery.Select(c => new CourseItemDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Level = c.Level.ToString(),
                IsPublished = c.IsPublished,
                Enrolled = false
            }).ToListAsync(ct);

            return new PagedResult<CourseItemDto> { Page = page, PageSize = pageSize, Total = total, Items = items };
        }
        else
        {
            var uid = currentUserId.Value;
            var items = await itemsQuery
                .Select(c => new CourseItemDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Level = c.Level.ToString(),
                    IsPublished = c.IsPublished,
                    Enrolled = _db.Enrollments.Any(e => e.CourseId == c.Id && e.UserId == uid)
                })
                .ToListAsync(ct);

            return new PagedResult<CourseItemDto> { Page = page, PageSize = pageSize, Total = total, Items = items };
        }
    }

    public async Task<bool> EnrollAsync(long courseId, long userId, CancellationToken ct)
    {
        var exists = await _db.Enrollments.AnyAsync(e => e.CourseId == courseId && e.UserId == userId, ct);
        if (exists) return true; // idempotent

        var courseExists = await _db.Courses.AnyAsync(c => c.Id == courseId && c.IsPublished, ct);
        if (!courseExists) return false;

        _db.Enrollments.Add(new Enrollments { CourseId = courseId, UserId = userId });
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UnenrollAsync(long courseId, long userId, CancellationToken ct)
    {
        var row = await _db.Enrollments.SingleOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId, ct);
        if (row is null) return true; // idempotent
        _db.Enrollments.Remove(row);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
