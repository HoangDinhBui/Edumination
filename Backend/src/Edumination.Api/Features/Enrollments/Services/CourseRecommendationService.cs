using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Features.Recommendations.Dtos;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Features.Recommendations.Services;

public interface ICourseRecommendationService
{
    Task<(decimal? targetBand, List<RecommendedCourseDto> items)> GetForUserAsync(
        long userId, CourseRecommendationQuery query, CancellationToken ct);
}

public class CourseRecommendationService : ICourseRecommendationService
{
    private readonly AppDbContext _db;
    public CourseRecommendationService(AppDbContext db) => _db = db;

    public async Task<(decimal? targetBand, List<RecommendedCourseDto> items)> GetForUserAsync(
        long userId, CourseRecommendationQuery query, CancellationToken ct)
    {
        // 1) Lấy band mục tiêu từ user_stats
        decimal? targetBand = await _db.UserStats
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .Select(s => s.BestBand)
            .SingleOrDefaultAsync(ct);

        // 2) Nếu có band -> gợi ý theo course_band_rules
        if (targetBand.HasValue)
        {
            var rulesQ =
                from r in _db.Set<CourseBandRule>().AsNoTracking()
                join c in _db.Courses.AsNoTracking() on r.CourseId equals c.Id
                where c.IsPublished
                   && targetBand.Value >= r.BandMin
                   && targetBand.Value <= r.BandMax
                select new { r, c };

            if (!string.IsNullOrWhiteSpace(query.level) &&
                Enum.TryParse<CourseLevel>(query.level, true, out var lvl))
            {
                rulesQ = rulesQ.Where(x => x.c.Level == lvl);
            }

            if (query.excludeEnrolled)
            {
                rulesQ = rulesQ.Where(x =>
                    !_db.Enrollments.Any(e => e.UserId == userId && e.CourseId == x.c.Id));
            }

            // xếp theo độ gần (khoảng cách từ mid-point của range tới targetBand), rồi UpdatedAt mới nhất
            var items = await rulesQ
                .OrderBy(x => Math.Abs(((x.r.BandMin + x.r.BandMax) / 2m) - targetBand.Value))
                .ThenByDescending(x => x.c.UpdatedAt)
                .Take(Math.Clamp(query.limit, 1, 50))
                .Select(x => new RecommendedCourseDto
                {
                    CourseId = x.c.Id,
                    Title = x.c.Title,
                    Description = x.c.Description,
                    Level = x.c.Level.ToString(),
                    IsPublished = x.c.IsPublished,
                    BandMin = x.r.BandMin,
                    BandMax = x.r.BandMax,
                    Distance = Math.Abs(((x.r.BandMin + x.r.BandMax) / 2m) - targetBand.Value)
                })
                .ToListAsync(ct);

            return (targetBand, items);
        }

        // 3) Nếu CHƯA có band -> gợi ý khóa dễ (BEGINNER/ELEMENTARY) & mới cập nhật
        var baseQ = _db.Courses.AsNoTracking().Where(c => c.IsPublished);

        if (!string.IsNullOrWhiteSpace(query.level) &&
            Enum.TryParse<CourseLevel>(query.level, true, out var lv2))
        {
            baseQ = baseQ.Where(c => c.Level == lv2);
        }
        else
        {
            baseQ = baseQ.Where(c =>
                c.Level == CourseLevel.BEGINNER || c.Level == CourseLevel.ELEMENTARY);
        }

        if (query.excludeEnrolled)
        {
            baseQ = baseQ.Where(c =>
                !_db.Enrollments.Any(e => e.UserId == userId && e.CourseId == c.Id));
        }

        var fallback = await baseQ
            .OrderByDescending(c => c.UpdatedAt)
            .ThenBy(c => c.Title)
            .Take(Math.Clamp(query.limit, 1, 50))
            .Select(c => new RecommendedCourseDto
            {
                CourseId = c.Id,
                Title = c.Title,
                Description = c.Description,
                Level = c.Level.ToString(),
                IsPublished = c.IsPublished,
                BandMin = 0.0m,
                BandMax = 3.5m,
                Distance = null
            })
            .ToListAsync(ct);

        return (null, fallback);
    }
}
