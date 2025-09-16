using System.Data;
using System.Threading.Tasks;
using Education.Repositories;
using Edumination.Api.Infrastructure.Persistence.Repositories;
using Edumination.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Edumination.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    TestPaperRepository TestPapers { get; }
    TestSectionRepository TestSections { get; }
    PassageRepository Passages { get; }
    QuestionRepository Questions { get; }
    AssetRepository Assets { get; }
    IBandScaleRepository BandScales { get; }
    ITestAttemptRepository TestAttempts { get; }
    ISectionAttemptRepository SectionAttempts { get; }
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
}
