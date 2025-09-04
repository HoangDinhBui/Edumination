using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Features.Courses.Services;
using Edumination.Api.Common.Results;
using FluentValidation;
using JwtNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Edumination.Api.Features.Courses;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _svc;
    private readonly IValidator<CreateCourseRequest> _validator;
    public CoursesController(ICourseService svc, IValidator<CreateCourseRequest> validator)
    {
        _svc = svc;
        _validator = validator;
    }

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

    [HttpGet("{id:long}", Name = "GetCourseById")]
    [AllowAnonymous] // public được; server sẽ ẩn nội dung nếu chưa đủ quyền
    [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Detail([FromRoute] long id, CancellationToken ct)
    {
        var dto = await _svc.GetDetailAsync(id, User, ct);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    [Authorize(Roles = "TEACHER,ADMIN")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResult<CreateCourseResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequest req, CancellationToken ct)
    {
        var val = await _validator.ValidateAsync(req, ct);
        if (!val.IsValid)
        {
            var errs = val.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            return BadRequest(new ApiResult<object>(false, null, System.Text.Json.JsonSerializer.Serialize(errs)));
        }

        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(new ApiResult<object>(false, null, "Invalid token."));

        var result = await _svc.CreateAsync(req, userId.Value, ct);
        if (!result.Success) return BadRequest(result);

        return CreatedAtRoute(
            routeName: "GetCourseById",
            routeValues: new { id = result.Data!.Id },
            value: result
        );
    }

    // helper: lấy userId từ token
    private long? GetUserId()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtNames.Sub);
        return long.TryParse(userIdStr, out var id) ? id : (long?)null;
    }
}
