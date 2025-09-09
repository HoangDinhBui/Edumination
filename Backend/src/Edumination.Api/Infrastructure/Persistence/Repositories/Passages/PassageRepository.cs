using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Persistence.Repositories;

public class PassageRepository : IPassageRepository
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

    public async Task<Passage> CreateAsync(Passage passage)
    {
        _context.Passages.Add(passage);
        await _context.SaveChangesAsync();
        return passage;
    }

    public async Task<Passage?> GetByIdAsync(long id)
    {
        return await _context.Passages.FindAsync(id);
    }

    public async Task<Passage?> GetBySectionIdAndPositionAsync(long sectionId, int position)
    {
        return await _context.Passages
            .FirstOrDefaultAsync(p => p.SectionId == sectionId && p.Position == position);
    }

    public async Task<Passage> UpdateAsync(Passage passage)
    {
        _context.Passages.Update(passage);
        await _context.SaveChangesAsync();
        return passage;
    }
    public async Task DeleteAsync(long id)
    {
        var passage = await _context.Passages.FindAsync(id);
        if (passage != null)
        {
            _context.Passages.Remove(passage);
            await _context.SaveChangesAsync();
        }
    }
}