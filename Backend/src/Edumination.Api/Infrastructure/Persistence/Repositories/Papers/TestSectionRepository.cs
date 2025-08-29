using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Entities;
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
        await _context.SaveChangesAsync();
        return testSection;
    }
}