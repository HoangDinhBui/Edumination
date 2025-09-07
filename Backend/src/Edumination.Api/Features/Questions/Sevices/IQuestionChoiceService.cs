// Edumination.Api.Services.Interfaces/IQuestionChoiceService.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Dtos;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Api.Services.Interfaces
{
    public interface IQuestionChoiceService
    {
        Task<QuestionChoice> CreateChoiceAsync(long questionId, QuestionChoiceCreateDto dto);
    }
}