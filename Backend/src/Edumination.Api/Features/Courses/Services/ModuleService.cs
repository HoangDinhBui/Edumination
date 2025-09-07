// Features/Courses/Services/IModuleService.cs
using System.Security.Claims;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Api.Features.Courses.Services
{
    public interface IModuleService
    {
        Task<List<LessonDto>?> GetLessonsAsync(long moduleId, ClaimsPrincipal? user, CancellationToken ct);
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
    }
}
