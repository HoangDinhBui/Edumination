using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Api.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly AppDbContext _context;

        public SectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TestSection> GetByIdAsync(long id)
        {
            return await _context.TestSections.FindAsync(id);
        }

        public async Task UpdateAsync(TestSection section)
        {
            _context.TestSections.Update(section);
            await _context.SaveChangesAsync();
        }
    }
}