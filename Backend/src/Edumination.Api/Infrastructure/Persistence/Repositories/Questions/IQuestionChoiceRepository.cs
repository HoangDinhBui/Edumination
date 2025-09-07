// Edumination.Api.Repositories.Interfaces/IQuestionChoiceRepository.cs
using Edumination.Api.Domain.Entities;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Api.Repositories.Interfaces
{
    public interface IQuestionChoiceRepository
    {
        Task<QuestionChoice> CreateAsync(QuestionChoice choice);
        Task<QuestionChoice> GetByQuestionIdAndPositionAsync(long questionId, int position);
    }
}