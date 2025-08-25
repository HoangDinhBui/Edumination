using Edumination.Api.Common.Results;
using Edumination.Api.Features.Auth.Requests;
using Edumination.Api.Features.Auth.Responses;
using Edumination.Api.Features.Auth.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        // nếu có model validation attributes/FluentValidation
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var resp = await _auth.LoginAsync(request, ct);
            return Ok(resp);
        }
        catch (UnauthorizedAccessException)
        {
            // Sai email/mật khẩu → 401
            return Unauthorized(new { message = "Invalid email or password" });
        }
        catch (InvalidOperationException ex)
        {
            // Lỗi cấu hình (ví dụ JWT key quá ngắn) → 500
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
        catch (Exception)
        {
            // Log chi tiết ex ở middleware/logger
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
        }
    }
}
