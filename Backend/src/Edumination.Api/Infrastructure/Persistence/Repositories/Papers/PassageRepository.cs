using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Persistence.Repositories;

public class PassageRepository
{
    private readonly AppDbContext _context;

    public PassageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Passage> AddAsync(Passage passage)
    {
        await _context.Passages.AddAsync(passage);
        return passage; // UnitOfWork sẽ xử lý SaveChanges
    }
}