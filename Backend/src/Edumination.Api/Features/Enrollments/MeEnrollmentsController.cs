using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Edumination.Api.Common.Results;
using Edumination.Api.Features.Enrollments.Dtos;
using Edumination.Api.Features.Enrollments.Services;
using Edumination.Api.Features.Admin.Dtos;
using Edumination.Api.Features.Recommendations.Dtos;
using Edumination.Api.Features.Recommendations.Services;

namespace Edumination.Api.Features.Enrollments;

[ApiController]
[Route("api/v1/me")]
public class MeController : ControllerBase
{
    private readonly IMyEnrollmentsService _enrollSvc;
    private readonly ICourseRecommendationService _recoSvc;

    public MeController(IMyEnrollmentsService enrollSvc, ICourseRecommendationService recoSvc)
    {
        _enrollSvc = enrollSvc;
        _recoSvc = recoSvc;
    }

    // GET /api/v1/me/enrollments?page=1&pageSize=20&q=&published=&level=
    [HttpGet("enrollments")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<MyEnrollmentItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> MyEnrollments([FromQuery] MyEnrollmentQuery query, CancellationToken ct)
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!long.TryParse(idStr, out var userId)) return Unauthorized(new { error = "Invalid token." });

        var result = await _enrollSvc.GetMineAsync(userId, query, ct);
        return Ok(result); // 200 + danh sách (có thể rỗng)
    }

    // GET /api/v1/me/recommendations/courses?limit=10&level=&excludeEnrolled=true
    [HttpGet("recommendations/courses")]
    [Authorize]
    [ProducesResponseType(typeof(List<RecommendedCourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecommendCourses([FromQuery] CourseRecommendationQuery query, CancellationToken ct)
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!long.TryParse(idStr, out var userId))
            return Unauthorized(new { error = "Invalid token." });

        var (targetBand, items) = await _recoSvc.GetForUserAsync(userId, query, ct);

        // Nếu muốn trả kèm targetBand
        return Ok(new
        {
            targetBand,
            count = items.Count,
            items
        });
    }
}
