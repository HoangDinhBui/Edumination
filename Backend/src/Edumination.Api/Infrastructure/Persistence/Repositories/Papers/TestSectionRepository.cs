using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Persistence.Repositories;

public class TestSectionRepository
{
    private readonly AppDbContext _context;

    public TestSectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TestSection> AddAsync(TestSection testSection)
    {
        await _context.TestSections.AddAsync(testSection);
        return testSection; // UnitOfWork sẽ xử lý SaveChanges
    }

    public IQueryable<TestSection> GetQueryable()
    {
        return _context.TestSections.AsQueryable();
    }

    public async Task<TestSection> GetByIdAsync(long id)
    {
        return await _context.TestSections.FindAsync(id);
    }
}