// Edumination.Api.Repositories.Interfaces/IQuestionRepository.cs
using Edumination.Api.Domain.Entities;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Api.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> CreateAsync(Question question);
        Task<Question> GetByIdAsync(long id);
        Task<Question> GetBySectionIdAndPositionAsync(long sectionId, int position);
        Task<List<QuestionChoice>> GetByQuestionIdAsync(long questionId, CancellationToken ct = default);
        Task AddAsync(QuestionChoice choice, CancellationToken ct = default);
    }
}