using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Features.Auth.Services;

namespace Edumination.Api.Features.Auth;

[ApiController]
[Route("api/v1/auth/oauth")]
public class OAuthController : ControllerBase
{
    private readonly IOAuthStartService _oauth;

    public OAuthController(IOAuthStartService oauth)
    {
        _oauth = oauth;
    }

    [HttpPost("{provider}/start")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Start([FromRoute] string provider, [FromQuery] string? returnUrl, CancellationToken ct)
    {
        var (authUrl, state) = await _oauth.StartAsync(provider, returnUrl, ct);
        return Ok(new { provider, state, authUrl, expiresIn = TimeSpan.FromMinutes(10).TotalSeconds });
    }
}