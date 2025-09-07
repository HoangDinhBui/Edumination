// Edumination.Api.Services/QuestionService.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Dtos;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Api.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IExerciseRepository _exerciseRepository; // Thêm repository cho Exercise

        public QuestionService(
            IQuestionRepository questionRepository,
            ISectionRepository sectionRepository,
            IExerciseRepository exerciseRepository) // Thêm dependency
        {
            _questionRepository = questionRepository;
            _sectionRepository = sectionRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<Question> CreateQuestionAsync(long sectionId, QuestionCreateDto dto)
        {
            // Kiểm tra section
            var section = await _sectionRepository.GetByIdAsync(sectionId);
            if (section == null)
            {
                throw new KeyNotFoundException($"Section with ID {sectionId} not found.");
            }

            // Kiểm tra exercise
            var exercise = await _exerciseRepository.GetByIdAsync(dto.ExerciseId);
            if (exercise == null)
            {
                throw new KeyNotFoundException($"Exercise with ID {dto.ExerciseId} not found.");
            }

            // Kiểm tra vị trí câu hỏi
            var existingQuestion = await _questionRepository.GetBySectionIdAndPositionAsync(sectionId, dto.Position);
            if (existingQuestion != null)
            {
                throw new InvalidOperationException($"A question already exists at position {dto.Position} for section {sectionId}.");
            }

            var question = new Question
            {
                SectionId = sectionId,
                Qtype = dto.Qtype,
                Stem = dto.Stem,
                Position = dto.Position,
                MetaJson = dto.MetaJson,
                CreatedAt = DateTime.UtcNow,
                ExerciseId = dto.ExerciseId // Gán ExerciseId từ DTO
            };

            return await _questionRepository.CreateAsync(question);
        }
    }
}