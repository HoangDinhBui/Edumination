using Edumination.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Api.Repositories.Interfaces
{
    public interface ISectionRepository
    {
        Task<TestSection> GetByIdAsync(long id);
        Task UpdateAsync(TestSection section);
    }
}