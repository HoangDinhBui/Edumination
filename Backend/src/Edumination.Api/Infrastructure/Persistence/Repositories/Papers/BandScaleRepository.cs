using Edumination.Domain.Entities;
using Edumination.Domain.Interfaces;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Persistence.Repositories;

public class BandScaleRepository : GenericRepository<BandScale>, IBandScaleRepository
{
    public BandScaleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<BandScale>> GetByPaperIdAndSkillAsync(long paperId, string skill)
    {
        return await _context.BandScales
            .Where(bs => bs.PaperId == paperId && bs.Skill == skill)
            .ToListAsync();
    }
}