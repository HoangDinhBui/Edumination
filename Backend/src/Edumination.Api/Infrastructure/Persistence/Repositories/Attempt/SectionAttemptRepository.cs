using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Interfaces;

namespace Edumination.Persistence.Repositories;

public class SectionAttemptRepository : GenericRepository<SectionAttempt>, ISectionAttemptRepository
{
    public SectionAttemptRepository(AppDbContext context) : base(context)
    {
    }
}