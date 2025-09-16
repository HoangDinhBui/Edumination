using Education.Repositories;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Infrastructure.Persistence.Repositories;
using Edumination.Domain.Interfaces;
using Edumination.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;

namespace Edumination.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            TestPapers = new TestPaperRepository(_context);
            TestSections = new TestSectionRepository(_context);
            Passages = new PassageRepository(_context);
            Questions = new QuestionRepository(_context);
            Assets = new AssetRepository(_context); // Đảm bảo AssetRepository được triển khai
            BandScales = new BandScaleRepository(_context);
            TestAttempts = new TestAttemptRepository(_context);
            SectionAttempts = new SectionAttemptRepository(_context);
        }

        public TestPaperRepository TestPapers { get; private set; }
        public TestSectionRepository TestSections { get; private set; }
        public PassageRepository Passages { get; private set; }
        public QuestionRepository Questions { get; private set; }
        public AssetRepository Assets { get; private set; }
        public IBandScaleRepository BandScales { get; private set; }
        public ITestAttemptRepository TestAttempts { get; }
    public ISectionAttemptRepository SectionAttempts { get; }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            _transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
            return _transaction;
        }
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