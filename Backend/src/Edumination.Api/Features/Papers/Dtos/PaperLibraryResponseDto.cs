namespace Edumination.Api.Features.Papers.Dtos;

public class PaperLibraryResponseDto
{
    public string Title { get; set; } // Tương ứng "currentTitle"
    public List<PaperLibraryItemDto> Items { get; set; } // Tương ứng "currentItems"
}