using Edumination.Api.Features.Admin.Dtos;
using Edumination.Api.Features.Enrollments.Dtos;

namespace Edumination.Api.Features.Enrollments.Services;

public interface IMyEnrollmentsService
{
    Task<PagedResult<MyEnrollmentItemDto>> GetMineAsync(
        long userId, MyEnrollmentQuery query, CancellationToken ct);
}