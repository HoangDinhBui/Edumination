using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TestPaper> TestPapers => Set<TestPaper>();
    public DbSet<TestSection> TestSections => Set<TestSection>();
    public DbSet<TestAttempt> TestAttempts => Set<TestAttempt>();
    public DbSet<SectionAttempt> SectionAttempts => Set<SectionAttempt>();
    protected override void OnModelCreating(ModelBuilder mb)
        => mb.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
