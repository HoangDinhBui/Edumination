using Edumination.Api.Features.Papers.Dtos;

namespace Edumination.Api.Features.Papers.Services;

public interface IPaperService
{
    Task<IReadOnlyList<PaperListItemDto>> ListPublishedAsync(CancellationToken ct);
}
