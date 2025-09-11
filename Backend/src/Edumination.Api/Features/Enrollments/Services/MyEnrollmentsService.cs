using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Edumination.Api.Common.Results;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Enrollments.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Features.Admin.Dtos;

namespace Edumination.Api.Features.Enrollments.Services;

public class MyEnrollmentsService : IMyEnrollmentsService
{
    private readonly AppDbContext _db;
    public MyEnrollmentsService(AppDbContext db) => _db = db;

    public async Task<PagedResult<MyEnrollmentItemDto>> GetMineAsync(
        long userId, MyEnrollmentQuery query, CancellationToken ct)
    {
        var baseQ =
            from e in _db.Enrollments.AsNoTracking()
            where e.UserId == userId
            join c in _db.Courses.AsNoTracking() on e.CourseId equals c.Id
            select new { e, c };

        if (!string.IsNullOrWhiteSpace(query.q))
            baseQ = baseQ.Where(x => x.c.Title.Contains(query.q!));

        if (query.published.HasValue)
            baseQ = baseQ.Where(x => x.c.IsPublished == query.published.Value);

        if (!string.IsNullOrWhiteSpace(query.level) &&
            Enum.TryParse<CourseLevel>(query.level, true, out var lvl))
        {
            baseQ = baseQ.Where(x => x.c.Level == lvl);
        }

        var total = await baseQ.CountAsync(ct);

        var page = Math.Max(1, query.page);
        var pageSize = Math.Clamp(query.pageSize, 5, 100);

        // Lấy danh sách course ở trang hiện tại
        var rows = await baseQ
            .OrderByDescending(x => x.e.EnrolledAt)
            .ThenBy(x => x.c.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MyEnrollmentItemDto
            {
                CourseId = x.c.Id,
                Title = x.c.Title,
                Description = x.c.Description,
                Level = x.c.Level.ToString(),
                IsPublished = x.c.IsPublished,
                EnrolledAt = x.e.EnrolledAt,

                // Đếm tổng bài học và số bài đã hoàn thành (theo user)
                TotalLessons = _db.Lessons
                    .Join(_db.Modules.Where(m => m.CourseId == x.c.Id),
                          l => l.ModuleId, m => m.Id, (l, m) => l.Id)
                    .Count(),

                CompletedLessons = _db.LessonCompletions
                    .Join(_db.Lessons, lc => lc.LessonId, l => l.Id, (lc, l) => new { lc, l })
                    .Join(_db.Modules.Where(m => m.CourseId == x.c.Id),
                          t => t.l.ModuleId, m => m.Id, (t, m) => t.lc)
                    .Count(lc => lc.UserId == userId)
            })
            .ToListAsync(ct);

        return new PagedResult<MyEnrollmentItemDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = rows
        };
    }
}
