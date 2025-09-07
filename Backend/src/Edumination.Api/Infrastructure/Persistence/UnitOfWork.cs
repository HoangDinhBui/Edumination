using Education.Repositories;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Infrastructure.Persistence.Repositories;
using Edumination.Domain.Interfaces;
using Edumination.Persistence.Repositories;
using System.Threading.Tasks;

namespace Edumination.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            TestPapers = new TestPaperRepository(_context);
            TestSections = new TestSectionRepository(_context);
            Passages = new PassageRepository(_context);
            Questions = new QuestionRepository(_context);
            Assets = new AssetRepository(_context); // Đảm bảo AssetRepository được triển khai
        }

        public TestPaperRepository TestPapers { get; private set; }
        public TestSectionRepository TestSections { get; private set; }
        public PassageRepository Passages { get; private set; }
        public QuestionRepository Questions { get; private set; }
        public AssetRepository Assets { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}