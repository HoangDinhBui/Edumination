namespace Edumination.WinForms.Dto.Papers
{
    public class PaperLibraryResponseDto
    {
        public string Title { get; set; } // Tương ứng "currentTitle"
        public List<PaperLibraryItemDto> Items { get; set; } // Tương ứng "currentItems"
    }
}

