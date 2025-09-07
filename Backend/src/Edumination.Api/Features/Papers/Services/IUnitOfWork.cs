using System.Threading.Tasks;
using Education.Repositories;
using Edumination.Api.Infrastructure.Persistence.Repositories;
using Edumination.Persistence.Repositories;

namespace Edumination.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    TestPaperRepository TestPapers { get; }
    TestSectionRepository TestSections { get; }
    PassageRepository Passages { get; }
    QuestionRepository Questions { get; }
    AssetRepository Assets { get; }
}
