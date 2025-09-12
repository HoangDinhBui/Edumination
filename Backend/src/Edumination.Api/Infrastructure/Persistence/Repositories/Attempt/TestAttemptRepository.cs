using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Domain.Interfaces;

namespace Edumination.Persistence.Repositories;

public class TestAttemptRepository : GenericRepository<TestAttempt>, ITestAttemptRepository
{
    public TestAttemptRepository(AppDbContext context) : base(context)
    {
    }
}