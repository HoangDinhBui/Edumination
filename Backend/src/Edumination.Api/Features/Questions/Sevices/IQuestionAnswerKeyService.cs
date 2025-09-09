using Edumination.Api.Dtos;

namespace Edumination.Api.Services.Interfaces
{
    public interface IQuestionAnswerKeyService
    {
        Task<QuestionAnswerKeyDto> CreateAnswerKeyAsync(long qid, QuestionAnswerKeyCreateDto dto);
        Task<QuestionAnswerKeyDto> UpdateAnswerKeyAsync(long qid, QuestionAnswerKeyUpdateDto dto);
        Task DeleteAnswerKeyAsync(long qid);
        Task<QuestionAnswerKeyDto> GetAnswerKeyAsync(long qid);
    }
}