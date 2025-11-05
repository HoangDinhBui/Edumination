namespace Edumination.Api.Features.Papers.Dtos;

public class PaperLibraryItemDto
{
    public long Id { get; set; }
    public string Name { get; set; } // Tương ứng "item.name" bên React
    public long Taken { get; set; } // Tương ứng "item.taken" bên React
}