using System.ComponentModel.DataAnnotations;

namespace Edumination.Api.Dtos
{
    public class UpdateSectionDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Time limit must be non-negative.")]
        public int? TimeLimitSec { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Invalid audio asset ID.")]
        public long? AudioAssetId { get; set; }

        public bool? IsPublished { get; set; }
    }
}