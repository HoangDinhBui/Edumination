// Edumination.Api.Services/QuestionChoiceService.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Dtos;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Api.Services.Interfaces;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Api.Services
{
    public class QuestionChoiceService : IQuestionChoiceService
    {
        private readonly IQuestionChoiceRepository _questionChoiceRepository;
        private readonly IQuestionRepository _questionRepository;

        public QuestionChoiceService(
            IQuestionChoiceRepository questionChoiceRepository,
            IQuestionRepository questionRepository)
        {
            _questionChoiceRepository = questionChoiceRepository;
            _questionRepository = questionRepository;
        }

        public async Task<QuestionChoice> CreateChoiceAsync(long questionId, QuestionChoiceCreateDto dto)
        {
            // Kiểm tra xem câu hỏi có tồn tại không
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"Question with ID {questionId} not found.");
            }

            // Kiểm tra xem vị trí đã tồn tại chưa
            var existingChoice = await _questionChoiceRepository.GetByQuestionIdAndPositionAsync(questionId, dto.Position);
            if (existingChoice != null)
            {
                throw new InvalidOperationException($"A choice already exists at position {dto.Position} for question {questionId}.");
            }

            var choice = new QuestionChoice
            {
                QuestionId = questionId,
                Content = dto.Content,
                Position = dto.Position,
                IsCorrect = dto.IsCorrect
            };

            return await _questionChoiceRepository.CreateAsync(choice);
        }
    }
}