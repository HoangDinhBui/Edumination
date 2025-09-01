using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Features.Auth.Services;
using Microsoft.Extensions.Caching.Memory;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Edumination.Api.Features.Auth.Responses;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Edumination.Api.Features.Auth;

[ApiController]
[Route("api/v1/auth/oauth")]
public class OAuthController : ControllerBase
{
    private readonly IOAuthService _oauth;
    private readonly IMemoryCache _cache;
    private readonly IHttpClientFactory _http;
    private readonly OAuthOptions _opt;
    private readonly AppDbContext _db;

    public OAuthController(IOAuthService oauth)
    {
        _oauth = oauth;
    }

    [HttpPost("{provider}/start")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Start([FromRoute] string provider, [FromQuery] string? returnUrl, CancellationToken ct)
    {
        var (authUrl, state) = await _oauth.StartAsync(provider, returnUrl, userId: null, ct);
        return Ok(new { provider, state, authUrl, expiresIn = (int)TimeSpan.FromMinutes(10).TotalSeconds });
    }

    [HttpGet("{provider}/callback")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Callback([FromRoute] string provider,
                                              [FromQuery] string code,
                                              [FromQuery] string state,
                                              CancellationToken ct)
    {
        var req = new Edumination.Api.Features.Auth.Services.OAuthCallbackRequest
        {
            Code = code,
            State = state
        };

        var resp = await _oauth.HandleCallbackAsync(provider, req, ct);
        return Ok(resp);
    }

    // POST /api/v1/auth/me/link-oauth/{provider}
    [Authorize]
    [HttpPost("me/link-oauth/{provider}")]
    public async Task<IActionResult> Link([FromRoute] string provider, [FromBody] Edumination.Api.Features.Auth.Services.OAuthCallbackRequest req, CancellationToken ct)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!long.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _oauth.LinkAsync(userId, provider, req, ct);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // DELETE /api/v1/auth/me/link-oauth/{provider}
    [Authorize]
    [HttpDelete("me/link-oauth/{provider}")]
    public async Task<IActionResult> Unlink([FromRoute] string provider, CancellationToken ct)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!long.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _oauth.UnlinkAsync(userId, provider, ct);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("me/link-oauth/{provider}/start")]
    public async Task<IActionResult> StartLink([FromRoute] string provider,
                                            [FromQuery] string? returnUrl,
                                            CancellationToken ct)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!long.TryParse(userIdStr, out var userId)) return Unauthorized();

        var (authUrl, state) = await _oauth.StartAsync(provider, returnUrl, userId, ct);
        return Ok(new { provider, state, authUrl, expiresIn = (int)TimeSpan.FromMinutes(10).TotalSeconds });
    }
}