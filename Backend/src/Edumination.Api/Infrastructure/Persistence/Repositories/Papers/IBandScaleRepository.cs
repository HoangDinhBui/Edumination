using Edumination.Domain.Entities;
using Edumination.Domain.Interfaces;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Persistence.Repositories;

public interface IBandScaleRepository : IGenericRepository<BandScale>
{
    Task<List<BandScale>> GetByPaperIdAndSkillAsync(long paperId, string skill);
}