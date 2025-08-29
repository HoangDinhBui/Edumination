using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Persistence.Repositories;

public class QuestionRepository
{
    private readonly AppDbContext _context;

    public QuestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Question> AddAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
        return question;
    }
}