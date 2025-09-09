using Edumination.Api.Dtos;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;

namespace Edumination.Api.Services
{
    public class QuestionAnswerKeyService : IQuestionAnswerKeyService
    {
        private readonly AppDbContext _context;

        public QuestionAnswerKeyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuestionAnswerKeyDto> CreateAnswerKeyAsync(long qid, QuestionAnswerKeyCreateDto dto)
        {
            var question = await _context.Questions.FindAsync(qid);
            if (question == null) throw new KeyNotFoundException("Không tìm thấy câu hỏi");

            var existingKey = await _context.QuestionAnswerKeys
                .FirstOrDefaultAsync(k => k.QuestionId == qid);
            if (existingKey != null) throw new InvalidOperationException("Khóa đáp án đã tồn tại cho câu hỏi này");

            var answerKey = new QuestionAnswerKey
            {
                QuestionId = qid,
                KeyJson = dto.KeyJson
            };
            _context.QuestionAnswerKeys.Add(answerKey);
            await _context.SaveChangesAsync();

            return new QuestionAnswerKeyDto
            {
                Id = answerKey.Id,
                QuestionId = qid,
                KeyJson = answerKey.KeyJson
            };
        }

        public async Task<QuestionAnswerKeyDto> UpdateAnswerKeyAsync(long qid, QuestionAnswerKeyUpdateDto dto)
        {
            var answerKey = await _context.QuestionAnswerKeys
                .FirstOrDefaultAsync(k => k.QuestionId == qid);
            if (answerKey == null) throw new KeyNotFoundException("Không tìm thấy khóa đáp án");

            answerKey.KeyJson = dto.KeyJson;
            await _context.SaveChangesAsync();

            return new QuestionAnswerKeyDto
            {
                Id = answerKey.Id,
                QuestionId = qid,
                KeyJson = answerKey.KeyJson
            };
        }

        public async Task DeleteAnswerKeyAsync(long qid)
        {
            var answerKey = await _context.QuestionAnswerKeys
                .FirstOrDefaultAsync(k => k.QuestionId == qid);
            if (answerKey == null) throw new KeyNotFoundException("Không tìm thấy khóa đáp án");

            _context.QuestionAnswerKeys.Remove(answerKey);
            await _context.SaveChangesAsync();
        }

        public async Task<QuestionAnswerKeyDto> GetAnswerKeyAsync(long qid)
        {
            var answerKey = await _context.QuestionAnswerKeys
                .FirstOrDefaultAsync(k => k.QuestionId == qid);
            if (answerKey == null) throw new KeyNotFoundException("Không tìm thấy khóa đáp án");

            return new QuestionAnswerKeyDto
            {
                Id = answerKey.Id,
                QuestionId = qid,
                KeyJson = answerKey.KeyJson
            };
        }
    }
}