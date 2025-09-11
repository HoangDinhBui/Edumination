// Edumination.Api.Infrastructure.Persistence/Repositories/QuestionChoiceRepository.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Api.Infrastructure.Persistence.Repositories
{
    public class QuestionChoiceRepository : IQuestionChoiceRepository
    {
        private readonly AppDbContext _context;

        public QuestionChoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuestionChoice> CreateAsync(QuestionChoice choice)
        {
            _context.QuestionChoices.Add(choice);
            await _context.SaveChangesAsync();
            return choice;
        }

        public async Task<QuestionChoice> GetByQuestionIdAndPositionAsync(long questionId, int position)
        {
            return await _context.QuestionChoices
                .FirstOrDefaultAsync(c => c.QuestionId == questionId && c.Position == position);
        }
    }
}