using Edumination.Api.Domain.Entities;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Domain.Repositories;

public interface ITestSectionRepository
{
    Task<TestSection> AddAsync(TestSection testSection);
}
