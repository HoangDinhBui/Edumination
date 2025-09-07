// Edumination.Api.Dtos/PassageUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Edumination.Api.Dtos
{
    public class PassageUpdateDto
    {
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Content text cannot exceed 1000 characters.")]
        public string? ContentText { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Invalid audio asset ID.")]
        public long? AudioId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Invalid transcript asset ID.")]
        public long? TranscriptId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Position must be non-negative.")]
        public int? Position { get; set; }
    }
}