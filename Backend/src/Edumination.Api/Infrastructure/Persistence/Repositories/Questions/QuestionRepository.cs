// Edumination.Api.Infrastructure.Persistence.Repositories/QuestionRepository.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Api.Infrastructure.Persistence.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Question> CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> GetByIdAsync(long id)
        {
            return await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question> GetBySectionIdAndPositionAsync(long sectionId, int position)
        {
            return await _context.Questions
                .FirstOrDefaultAsync(q => q.SectionId == sectionId && q.Position == position);
        }

        public async Task<List<QuestionChoice>> GetByQuestionIdAsync(long questionId, CancellationToken ct = default)
        {
            return await _context.QuestionChoices
                .Where(qc => qc.QuestionId == questionId)
                .OrderBy(qc => qc.Position)
                .ToListAsync(ct);
        }

        public async Task AddAsync(QuestionChoice choice, CancellationToken ct = default)
        {
            _context.QuestionChoices.Add(choice);
            await _context.SaveChangesAsync(ct);
        }
    }
}