// Edumination.Api.Dtos/QuestionChoiceCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Edumination.Api.Dtos
{
    public class QuestionChoiceCreateDto
    {
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string Content { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Position must be non-negative.")]
        public int Position { get; set; }

        [Required(ErrorMessage = "IsCorrect flag is required.")]
        public bool IsCorrect { get; set; }
    }
}