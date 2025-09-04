using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Features.Courses.Services;

namespace Edumination.Api.Features.Courses;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _svc;
    public CoursesController(ICourseService svc) => _svc = svc;

    // GET /api/v1/courses?published=1&q=&level=&page=1&pageSize=20
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<CourseItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] CourseListQuery query, CancellationToken ct)
    {
        var result = await _svc.GetAsync(query, User, ct);
        return Ok(result);
    }

    // POST /api/v1/courses/{id}/enroll
    [HttpPost("{id:long}/enroll")]
    [Authorize]
    public async Task<IActionResult> Enroll([FromRoute] long id, CancellationToken ct)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!long.TryParse(userIdStr, out var userId)) return Unauthorized();

        var ok = await _svc.EnrollAsync(id, userId, ct);
        if (!ok) return NotFound(new { error = "Course not found or not published." });
        return Ok(new { success = true });
    }

    // DELETE /api/v1/courses/{id}/enroll
    [HttpDelete("{id:long}/enroll")]
    [Authorize]
    public async Task<IActionResult> Unenroll([FromRoute] long id, CancellationToken ct)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!long.TryParse(userIdStr, out var userId)) return Unauthorized();

        var ok = await _svc.UnenrollAsync(id, userId, ct);
        if (!ok) return NotFound(new { error = "Course not found." });
        return Ok(new { success = true });
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous] // public được; server sẽ ẩn nội dung nếu chưa đủ quyền
    [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Detail([FromRoute] long id, CancellationToken ct)
    {
        var dto = await _svc.GetDetailAsync(id, User, ct);
        if (dto is null) return NotFound();
        return Ok(dto);
    }
}
