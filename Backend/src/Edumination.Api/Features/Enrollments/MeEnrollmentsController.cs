using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Edumination.Api.Common.Results;
using Edumination.Api.Features.Enrollments.Dtos;
using Edumination.Api.Features.Enrollments.Services;
using Edumination.Api.Features.Admin.Dtos;

namespace Edumination.Api.Features.Stats;

[ApiController]
[Route("api/v1/me")]
public class MeController : ControllerBase
{
    private readonly IMyEnrollmentsService _enrollSvc;

    public MeController(IMyEnrollmentsService enrollSvc)
    {
        _enrollSvc = enrollSvc;
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
}
