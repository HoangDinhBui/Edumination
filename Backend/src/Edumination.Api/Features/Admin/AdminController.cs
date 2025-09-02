using Edumination.Api.Common.Results;
using Edumination.Api.Features.Admin.Dtos;
using Edumination.Api.Features.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edumination.Api.Features.Admin;

[ApiController]
[Route("api/v1/admin/users")]
[Authorize(Roles = "ADMIN")]
public class AdminUsersController : ControllerBase
{
    private readonly IAdminUsersService _svc;
    public AdminUsersController(IAdminUsersService svc) { _svc = svc; }

    // GET /api/v1/admin/users?email=&role=&active=&page=&pageSize=
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AdminUserItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] AdminUserQuery q, CancellationToken ct)
    {
        var result = await _svc.GetUsersAsync(q, ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResult<CreateUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest req, CancellationToken ct)
    {
        var result = await _svc.CreateUserAsync(req, ct);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // PATCH /api/v1/admin/users/{id}
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResult<AdminUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch([FromRoute] long id, [FromBody] PatchUserRequest req, CancellationToken ct)
    {
        var result = await _svc.PatchUserAsync(id, req, ct);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
