// Edumination.Api.Dtos/QuestionCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Edumination.Api.Dtos
{
    public class QuestionCreateDto
    {
        [Required(ErrorMessage = "Question type is required.")]
        [StringLength(50, ErrorMessage = "Question type cannot exceed 50 characters.")]
        public string Qtype { get; set; }

        [Required(ErrorMessage = "Stem is required.")]
        [StringLength(1000, ErrorMessage = "Stem cannot exceed 1000 characters.")]
        public string Stem { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Position must be non-negative.")]
        public int Position { get; set; }

        [StringLength(2000, ErrorMessage = "Meta JSON cannot exceed 2000 characters.")]
        public string? MetaJson { get; set; }

        [Required(ErrorMessage = "Exercise ID is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Exercise ID must be a positive number.")]
        public long ExerciseId { get; set; } // Thêm trường này
    }
}