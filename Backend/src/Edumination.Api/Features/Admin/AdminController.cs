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
}
