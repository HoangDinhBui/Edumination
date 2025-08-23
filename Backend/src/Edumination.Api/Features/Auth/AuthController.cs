using Edumination.Api.Common.Results;
using Edumination.Api.Features.Auth.Requests;
using Edumination.Api.Features.Auth.Responses;
using Edumination.Api.Features.Auth.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Edumination.Api.Features.Auth;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IValidator<RegisterRequest> _registerValidator;

    public AuthController(IAuthService auth, IValidator<RegisterRequest> registerValidator)
    {
        _auth = auth;
        _registerValidator = registerValidator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResult<RegisterResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        var val = await _registerValidator.ValidateAsync(req, ct);
        if (!val.IsValid)
        {
            var errors = val.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            return BadRequest(new ApiResult<object>(false, null, System.Text.Json.JsonSerializer.Serialize(errors)));
        }

        var result = await _auth.RegisterAsync(req, ct);
        if (!result.Success) return Conflict(result);
        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var response = await _auth.LoginAsync(request, ct);
        return Ok(response);
    }
}
