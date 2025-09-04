using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Persistence.Repositories;

public class TestPaperRepository
{
    private readonly AppDbContext _context;

    public TestPaperRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TestPaper> AddAsync(TestPaper testPaper)
    {
        await _context.TestPapers.AddAsync(testPaper);
        return testPaper; // UnitOfWork sẽ xử lý SaveChanges
    }

    public async Task<TestPaper> GetByIdAsync(long id)
    {
        return await _context.TestPapers
            .Include(t => t.PdfAsset)
            .Include(t => t.TestSections)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}