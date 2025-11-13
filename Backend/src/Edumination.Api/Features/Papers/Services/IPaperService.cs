using Edumination.Api.Features.Papers.Dtos;

namespace Edumination.Api.Features.Papers.Services;

public interface IPaperService
{
    Task<IReadOnlyList<PaperListItemDto>> ListPublishedAsync(CancellationToken ct);
    Task<PaperLibraryResponseDto> ListAsync(
        string? status, 
        string? skill, 
        string? search, 
        string? sort, 
        bool isTeacherOrAdmin, 
        CancellationToken ct = default
    );

    // Thêm: Get detailed paper (với tùy chọn ẩn answers)
    Task<DetailedPaperDto?> GetDetailedAsync(long id, bool hideAnswers, CancellationToken ct = default);
}
