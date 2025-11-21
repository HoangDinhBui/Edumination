namespace Edumination.Api.Features.Papers.Dtos;

public class MockTestLibraryResponseDto
{
    public string Title { get; set; } = "";
    public List<PaperLibraryItemDto> Items { get; set; } = new();
}