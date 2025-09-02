using Edumination.Api.Common.Results;
using Edumination.Api.Features.Admin.Dtos;
using Edumination.Api.Features.Admin.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edumination.Api.Features.Admin;

[ApiController]
[Route("api/v1/admin/edu-domains")]
[Authorize(Roles = "ADMIN")]
public class AdminEduDomainsController : ControllerBase
{
    private readonly IEduDomainService _svc;
    private readonly IValidator<CreateEduDomainRequest> _createValidator;

    public AdminEduDomainsController(IEduDomainService svc, IValidator<CreateEduDomainRequest> createValidator)
    {
        _svc = svc;
        _createValidator = createValidator;
    }

    // GET /admin/edu-domains?q=&active=&page=&pageSize=
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<EduDomainItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] EduDomainListQuery query, CancellationToken ct)
        => Ok(await _svc.GetAsync(query, ct));

    // POST /admin/edu-domains
    [HttpPost]
    [ProducesResponseType(typeof(ApiResult<EduDomainItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateEduDomainRequest req, CancellationToken ct)
    {
        var val = await _createValidator.ValidateAsync(req, ct);
        if (!val.IsValid)
        {
            var errors = val.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            return BadRequest(new ApiResult<object>(false, null, System.Text.Json.JsonSerializer.Serialize(errors)));
        }

        var result = await _svc.CreateAsync(req, ct);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // DELETE /admin/edu-domains/{id}
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct)
    {
        var result = await _svc.DeleteAsync(id, ct);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }
}
