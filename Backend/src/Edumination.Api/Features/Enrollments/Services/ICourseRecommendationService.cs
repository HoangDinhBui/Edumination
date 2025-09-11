using Edumination.Api.Features.Recommendations.Dtos;

namespace Edumination.Api.Features.Recommendations.Services;

public interface ICourseRecommendationService
{
    Task<(decimal? targetBand, List<RecommendedCourseDto> items)> GetForUserAsync(
        long userId, CourseRecommendationQuery query, CancellationToken ct);
}