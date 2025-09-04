using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Common.Results;

namespace Edumination.Api.Features.Courses.Services;

public interface ICourseService
{
    Task<PagedResult<CourseItemDto>> GetAsync(CourseListQuery query, ClaimsPrincipal? user, CancellationToken ct);
    Task<bool> EnrollAsync(long courseId, long userId, CancellationToken ct);
    Task<bool> UnenrollAsync(long courseId, long userId, CancellationToken ct);
    Task<CourseDetailDto?> GetDetailAsync(long id, ClaimsPrincipal? user, CancellationToken ct);
    Task<ApiResult<CreateCourseResponse>> CreateAsync(CreateCourseRequest req, long creatorUserId, CancellationToken ct);
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

    public async Task<CourseDetailDto?> GetDetailAsync(long id, ClaimsPrincipal? user, CancellationToken ct)
    {
        var course = await _db.Courses.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        if (course is null) return null;

        // Resolve user + quyền
        long? uid = null;
        if (user is not null)
        {
            var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            if (long.TryParse(idStr, out var parsed)) uid = parsed;
        }

        var isStaff = user?.IsInRole("ADMIN") == true || user?.IsInRole("TEACHER") == true;
        var isOwner = uid.HasValue && course.CreatedBy == uid.Value;

        // Ẩn khóa chưa publish với người thường
        if (!course.IsPublished && !(isOwner || isStaff))
            return null; // Controller -> 404

        // Kiểm tra enroll
        var enrolled = false;
        if (uid.HasValue)
        {
            enrolled = await _db.Enrollments
                .AnyAsync(e => e.CourseId == id && e.UserId == uid.Value, ct);
        }

        var canViewContent = enrolled || isOwner || isStaff;

        // Lấy module + lesson (chỉ outline cho người chưa có quyền)
        // - Nếu chưa có quyền: chỉ lấy lesson published
        // - Nếu có quyền: lấy tất cả lesson (cả unpublished, để giáo viên/owner xem draft)
        var lessonsQuery = _db.Lessons.AsNoTracking()
            .Join(_db.Modules.AsNoTracking().Where(m => m.CourseId == id),
                  l => l.ModuleId, m => m.Id, (l, m) => new { l, m });

        if (!canViewContent)
        {
            lessonsQuery = lessonsQuery.Where(x => x.l.IsPublished);
        }

        var raw = await lessonsQuery
            .OrderBy(x => x.m.Position)
            .ThenBy(x => x.l.Position)
            .Select(x => new
            {
                ModuleId = x.m.Id,
                ModuleTitle = x.m.Title,
                ModulePos = x.m.Position,

                LessonId = x.l.Id,
                LessonTitle = x.l.Title,
                LessonPos = x.l.Position,
                x.l.IsPublished,
                x.l.Objective,
                x.l.VideoId,
                x.l.TranscriptId
            })
            .ToListAsync(ct);

        // group theo module
        var modules = raw
            .GroupBy(r => new { r.ModuleId, r.ModuleTitle, r.ModulePos })
            .Select(g => new ModuleDto
            {
                Id = g.Key.ModuleId,
                Title = g.Key.ModuleTitle,
                Position = g.Key.ModulePos,
                Lessons = g.Select(r => new LessonDto
                {
                    Id = r.LessonId,
                    Title = r.LessonTitle,
                    Position = r.LessonPos,
                    IsPublished = r.IsPublished,
                    Objective = canViewContent ? r.Objective : null,
                    VideoId = canViewContent ? r.VideoId : null,
                    TranscriptId = canViewContent ? r.TranscriptId : null
                }).ToList()
            })
            .OrderBy(m => m.Position)
            .ToList();

        // Tiến độ (nếu có uid)
        int? total = null, completed = null;
        if (uid.HasValue)
        {
            total = await _db.Lessons.AsNoTracking()
                .Join(_db.Modules.AsNoTracking().Where(m => m.CourseId == id),
                      l => l.ModuleId, m => m.Id, (l, m) => l.Id)
                .CountAsync(ct);

            completed = await _db.LessonCompletions.AsNoTracking()
                .Join(_db.Lessons.AsNoTracking(), lc => lc.LessonId, l => l.Id, (lc, l) => new { lc, l })
                .Join(_db.Modules.AsNoTracking().Where(m => m.CourseId == id),
                      t => t.l.ModuleId, m => m.Id, (t, m) => t.lc)
                .CountAsync(x => x.UserId == uid.Value, ct);
        }

        return new CourseDetailDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Level = course.Level.ToString(),
            IsPublished = course.IsPublished,
            Enrolled = enrolled,
            CanViewContent = canViewContent,
            Modules = modules,
            TotalLessons = total,
            CompletedLessons = completed
        };
    }

    public async Task<ApiResult<CreateCourseResponse>> CreateAsync(
    CreateCourseRequest req, long creatorUserId, CancellationToken ct)
    {
        // validate cơ bản
        if (string.IsNullOrWhiteSpace(req.Title))
            return new(false, null, "Title is required.");

        // parse string -> enum (mặc định BEGINNER nếu không hợp lệ)
        if (!Enum.TryParse<CourseLevel>((req.Level ?? "BEGINNER").Trim(), ignoreCase: true, out var levelEnum))
            levelEnum = CourseLevel.BEGINNER;

        var course = new Course
        {
            Title = req.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
            Level = levelEnum,                 // enum
            IsPublished = req.IsPublished,
            CreatedBy = creatorUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Courses.Add(course);
        await _db.SaveChangesAsync(ct);

        var resp = new CreateCourseResponse
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Level = course.Level.ToString(),
            IsPublished = course.IsPublished,
            CreatedBy = course.CreatedBy,
            CreatedAt = course.CreatedAt
        };

        return new(true, resp, null);
    }
}
