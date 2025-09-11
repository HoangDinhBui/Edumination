using Edumination.Api.Common.Results;
using Edumination.Api.Features.Admin.Dtos;

namespace Edumination.Api.Features.Admin.Services;

public interface IEduDomainService
{
    Task<PagedResult<EduDomainItemDto>> GetAsync(EduDomainListQuery q, CancellationToken ct);
    Task<ApiResult<EduDomainItemDto>> CreateAsync(CreateEduDomainRequest req, CancellationToken ct);
    Task<ApiResult<object>> DeleteAsync(long id, CancellationToken ct);
}