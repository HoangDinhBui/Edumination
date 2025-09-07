// Features/Courses/Services/IModuleService.cs
using System.Security.Claims;
using Edumination.Api.Common.Results;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Api.Features.Courses.Services
{
    public interface IModuleService
    {
        Task<List<LessonDto>?> GetLessonsAsync(long moduleId, ClaimsPrincipal? user, CancellationToken ct);
        Task<ApiResult<LessonDto>> CreateLessonAsync(
            long moduleId, CreateLessonRequest req, ClaimsPrincipal user, CancellationToken ct);
    }

    public class ModuleService : IModuleService
    {
        private readonly AppDbContext _db;
        public ModuleService(AppDbContext db) => _db = db;

        public async Task<List<LessonDto>?> GetLessonsAsync(long moduleId, ClaimsPrincipal? user, CancellationToken ct)
        {
            // 1) Lấy module (CourseId) + course (CreatedBy, IsPublished)
            var mod = await _db.Modules.AsNoTracking()
                .Where(m => m.Id == moduleId)
                .Select(m => new { m.Id, m.CourseId })
                .SingleOrDefaultAsync(ct);

            if (mod is null) return null;

            var course = await _db.Courses.AsNoTracking()
                .Where(c => c.Id == mod.CourseId)
                .Select(c => new { c.Id, c.CreatedBy, c.IsPublished })
                .SingleOrDefaultAsync(ct);

            if (course is null) return null;

            // 2) Resolve uid + role
            long? uid = null;
            if (user is not null)
            {
                var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
                if (long.TryParse(idStr, out var parsed)) uid = parsed;
            }

            var isStaff = user?.IsInRole("ADMIN") == true || user?.IsInRole("TEACHER") == true;
            var isOwner = uid.HasValue && course.CreatedBy == uid.Value;

            // 3) Ẩn module nếu course chưa publish và người dùng không đủ quyền
            if (!course.IsPublished && !(isOwner || isStaff))
                return null; // Controller -> 404

            // 4) Xác định quyền xem nội dung draft
            var enrolled = false;
            if (uid.HasValue)
            {
                enrolled = await _db.Enrollments.AsNoTracking()
                    .AnyAsync(e => e.CourseId == course.Id && e.UserId == uid.Value, ct);
            }
            var canViewContent = enrolled || isOwner || isStaff;

            // 5) Query lessons theo quyền
            var lq = _db.Lessons.AsNoTracking()
                .Where(l => l.ModuleId == moduleId);

            if (!canViewContent)
                lq = lq.Where(l => l.IsPublished);

            var lessons = await lq
                .OrderBy(l => l.Position)
                .Select(l => new LessonDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Position = l.Position,
                    IsPublished = l.IsPublished,
                    Objective = canViewContent ? l.Objective : null,
                    VideoId = canViewContent ? l.VideoId : null,
                    TranscriptId = canViewContent ? l.TranscriptId : null
                })
                .ToListAsync(ct);

            return lessons;
        }

        public async Task<ApiResult<LessonDto>> CreateLessonAsync(
            long moduleId, CreateLessonRequest req, ClaimsPrincipal user, CancellationToken ct)
        {
            // 1) Tìm module + course để kiểm tra quyền
            var mod = await _db.Modules
                .AsNoTracking()
                .Where(m => m.Id == moduleId)
                .Select(m => new { m.Id, m.CourseId })
                .SingleOrDefaultAsync(ct);
            if (mod is null) return new(false, null, "NOT_FOUND: Module");

            var course = await _db.Courses
                .SingleOrDefaultAsync(c => c.Id == mod.CourseId, ct);
            if (course is null) return new(false, null, "NOT_FOUND: Course");

            // 2) Quyền: ADMIN/TEACHER hoặc Owner
            long? uid = null;
            var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            if (long.TryParse(idStr, out var parsed)) uid = parsed;

            var isStaff = user.IsInRole("ADMIN") || user.IsInRole("TEACHER");
            var isOwner = uid.HasValue && course.CreatedBy == uid.Value;
            if (!(isStaff || isOwner))
                return new(false, null, "FORBIDDEN");

            // 3) Optional: kiểm tra Asset tồn tại nếu được truyền
            if (req.VideoId.HasValue)
            {
                var exist = await _db.Assets.AnyAsync(a => a.Id == req.VideoId.Value, ct);
                if (!exist) return new(false, null, "ASSET_NOT_FOUND: VideoId");
            }
            if (req.TranscriptId.HasValue)
            {
                var exist = await _db.Assets.AnyAsync(a => a.Id == req.TranscriptId.Value, ct);
                if (!exist) return new(false, null, "ASSET_NOT_FOUND: TranscriptId");
            }

            // 4) Tính position mục tiêu
            var maxPos = await _db.Lessons
                .Where(l => l.ModuleId == moduleId)
                .MaxAsync(l => (int?)l.Position, ct);
            int targetPos;
            if (!req.Position.HasValue || req.Position!.Value <= 0)
                targetPos = (maxPos ?? 0) + 1; // append
            else
                targetPos = Math.Min(req.Position.Value, (maxPos ?? 0) + 1); // clamp về cuối nếu > max+1

            // 5) Transaction: shift những lesson có position >= targetPos
            using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE lessons
                SET position = position + 1
                WHERE module_id = {moduleId} AND position >= {targetPos};", ct);

                var entity = new Lesson // hoặc Lesson nếu entity của bạn tên số ít
                {
                    ModuleId = moduleId,
                    Title = req.Title.Trim(),
                    Objective = string.IsNullOrWhiteSpace(req.Objective) ? null : req.Objective.Trim(),
                    VideoId = req.VideoId,
                    TranscriptId = req.TranscriptId,
                    Position = targetPos,
                    IsPublished = req.IsPublished ?? false,
                    CreatedBy = uid ?? 0,                 // nếu schema yêu cầu not null
                    CreatedAt = DateTime.UtcNow
                };

                _db.Lessons.Add(entity);

                // Cập nhật UpdatedAt course để phản ánh thay đổi nội dung
                course.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                var dto = new LessonDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Position = entity.Position,
                    IsPublished = entity.IsPublished,
                    Objective = entity.Objective,
                    VideoId = entity.VideoId,
                    TranscriptId = entity.TranscriptId
                };

                return new(true, dto, null);
            }
            catch (DbUpdateException)
            {
                await tx.RollbackAsync(ct);
                // Trường hợp hiếm race condition gây đụng UNIQUE (module_id, position)
                return new(false, null, "CONFLICT: Duplicate position.");
            }
        }
    }
}
