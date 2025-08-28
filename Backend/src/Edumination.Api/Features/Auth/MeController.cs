using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Features.Auth.Services;
using Edumination.Api.Features.Auth.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Edumination.Api.Features.Auth;

[ApiController]
[Route("api/v1/auth")]
public class MeController(IAuthService auth) : ControllerBase
{
    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(MeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var resp = await auth.GetMeAsync(User, ct);
        return Ok(resp);
    }
}
