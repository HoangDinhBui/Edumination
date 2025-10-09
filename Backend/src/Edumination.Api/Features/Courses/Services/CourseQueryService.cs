using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Common.Results;

namespace Edumination.Api.Features.Courses.Services;

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

        _db.Enrollments.Add(new Enrollment { CourseId = courseId, UserId = userId });
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

    public async Task<ApiResult<CourseDetailDto>> UpdatePartialAsync(
    long id, UpdateCourseRequest req, ClaimsPrincipal user, CancellationToken ct)
    {
        var course = await _db.Courses.SingleOrDefaultAsync(c => c.Id == id, ct);
        if (course is null) return new(false, null, "NOT_FOUND");

        // resolve uid + role
        long? uid = null;
        var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (long.TryParse(idStr, out var parsed)) uid = parsed;

        var isStaff = user.IsInRole("ADMIN") || user.IsInRole("TEACHER");
        var isOwner = uid.HasValue && course.CreatedBy == uid.Value;

        if (!(isStaff || isOwner))
            return new(false, null, "FORBIDDEN");

        // Apply partial updates
        if (req.Title != null)
        {
            var trimmed = req.Title.Trim();
            if (trimmed.Length == 0)
                return new(false, null, "Title cannot be empty.");
            course.Title = trimmed;
        }

        if (req.Description != null)
        {
            // empty string => NULL để "xoá mô tả"
            course.Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim();
        }

        if (req.Level != null)
        {
            if (!Enum.TryParse<CourseLevel>(req.Level.Trim(), true, out var lvl))
                return new(false, null, "Invalid level. Allowed: BEGINNER, ELEMENTARY, PRE_INTERMEDIATE, INTERMEDIATE, UPPER_INTERMEDIATE, ADVANCED.");
            course.Level = lvl;
        }

        if (req.IsPublished.HasValue)
        {
            course.IsPublished = req.IsPublished.Value;
        }

        course.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        // Trả lại detail (tôn trọng quyền xem nội dung)
        var dto = await GetDetailAsync(id, user, ct);
        return new(true, dto!, null);
    }

    public async Task<List<ModuleDto>?> GetModulesAsync(long courseId, ClaimsPrincipal? user, CancellationToken ct)
    {
        // 1) Tải course + check quyền xem outline
        var course = await _db.Courses.AsNoTracking().SingleOrDefaultAsync(c => c.Id == courseId, ct);
        if (course is null) return null;

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

        // 2) Xác định quyền xem nội dung (draft lessons)
        var enrolled = false;
        if (uid.HasValue)
        {
            enrolled = await _db.Enrollments.AsNoTracking()
                .AnyAsync(e => e.CourseId == courseId && e.UserId == uid.Value, ct);
        }
        var canViewContent = enrolled || isOwner || isStaff;

        // 3) Lấy modules
        var modules = await _db.Modules.AsNoTracking()
            .Where(m => m.CourseId == courseId)
            .OrderBy(m => m.Position)
            .Select(m => new { m.Id, m.Title, m.Description, m.Position })
            .ToListAsync(ct);

        if (modules.Count == 0)
            return new List<ModuleDto>();

        var moduleIds = modules.Select(m => m.Id).ToList();

        // 4) Lấy lessons (lọc đã publish nếu không có quyền)
        var lessonsQ = _db.Lessons.AsNoTracking()
            .Where(l => moduleIds.Contains(l.ModuleId));

        if (!canViewContent)
            lessonsQ = lessonsQ.Where(l => l.IsPublished);

        var lessons = await lessonsQ
            .OrderBy(l => l.ModuleId).ThenBy(l => l.Position)
            .Select(l => new
            {
                l.Id,
                l.Title,
                l.Position,
                l.IsPublished,
                l.ModuleId,
                l.Objective,
                l.VideoId,
                l.TranscriptId
            })
            .ToListAsync(ct);

        // 5) Group vào DTO (ẩn field nhạy cảm khi không có quyền)
        var lessonLookup = lessons.GroupBy(x => x.ModuleId)
            .ToDictionary(g => g.Key, g => g.Select(r => new LessonDto
            {
                Id = r.Id,
                Title = r.Title,
                Position = r.Position,
                IsPublished = r.IsPublished,
                Objective = canViewContent ? r.Objective : null,
                VideoId = canViewContent ? r.VideoId : null,
                TranscriptId = canViewContent ? r.TranscriptId : null
            }).OrderBy(l => l.Position).ToList());

        var result = modules.Select(m => new ModuleDto
        {
            Id = m.Id,
            Title = m.Title,
            Position = m.Position,
            Lessons = lessonLookup.TryGetValue(m.Id, out var ls) ? ls : new List<LessonDto>()
        })
        .OrderBy(x => x.Position)
        .ToList();

        return result;
    }

    public async Task<ApiResult<ModuleDto>> CreateModuleAsync(
    long courseId, CreateModuleRequest req, ClaimsPrincipal user, CancellationToken ct)
    {
        // 1) Tìm course
        var course = await _db.Courses.SingleOrDefaultAsync(c => c.Id == courseId, ct);
        if (course is null) return new(false, null, "NOT_FOUND");

        // 2) Quyền: ADMIN/TEACHER hoặc Owner
        long? uid = null;
        var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (long.TryParse(idStr, out var parsed)) uid = parsed;

        var isStaff = user.IsInRole("ADMIN") || user.IsInRole("TEACHER");
        var isOwner = uid.HasValue && course.CreatedBy == uid.Value;
        if (!(isStaff || isOwner)) return new(false, null, "FORBIDDEN");

        // 3) Tính position mục tiêu
        int targetPos;
        if (!req.Position.HasValue || req.Position!.Value <= 0)
        {
            var maxPos = await _db.Modules
                .Where(m => m.CourseId == courseId)
                .MaxAsync(m => (int?)m.Position, ct);
            targetPos = (maxPos ?? 0) + 1; // append
        }
        else
        {
            targetPos = req.Position.Value;
        }

        // 4) Transaction: shift các module >= targetPos lên +1 (để tránh UNIQUE (course_id, position) đụng nhau)
        using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            if (targetPos > 0)
            {
                // dịch chuyển hàng loạt chỉ trong course này
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE modules
                    SET position = position + 1
                    WHERE course_id = {courseId} AND position >= {targetPos};", ct);
            }

            var entity = new Module   // chú ý: entity của bạn là 'Modules' (đúng như bạn đang dùng _db.Modules)
            {
                CourseId = courseId,
                Title = req.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
                Position = targetPos
            };

            _db.Modules.Add(entity);

            // cập nhật UpdatedAt của course cho đúng semantics
            course.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            var dto = new ModuleDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Position = entity.Position,
                Lessons = new List<LessonDto>()
            };

            return new(true, dto, null);
        }
        catch (DbUpdateException)
        {
            await tx.RollbackAsync(ct);
            // có thể là lỗi UNIQUE (course_id, position) hiếm gặp nếu race condition
            return new(false, null, "CONFLICT: Duplicate position.");
        }
    }
}
