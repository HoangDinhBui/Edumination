using Edumination.Api.Common.Results;
using Edumination.Api.Features.Auth.Requests;
using Edumination.Api.Features.Auth.Responses;
using Edumination.Api.Features.Auth.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Edumination.Api.Features.Auth;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService, IValidator<RegisterRequest> registerValidator) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResult<RegisterResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        var val = await registerValidator.ValidateAsync(req, ct);
        if (!val.IsValid)
        {
            var errors = val.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            return BadRequest(new ApiResult<object>(false, null, System.Text.Json.JsonSerializer.Serialize(errors)));
        }

        var result = await authService.RegisterAsync(req, ct);
        if (!result.Success)
            return Conflict(result); // 409 nếu email trùng

        return Ok(result);
    }
}
