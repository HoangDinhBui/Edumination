// Edumination.Api.Services.Interfaces/IQuestionService.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Dtos;
using System.Threading.Tasks;

namespace Edumination.Api.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> CreateQuestionAsync(long sectionId, QuestionCreateDto dto);
    }
}