using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Domain.Entities.Leaderboard;
namespace Edumination.Api.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<EmailVerification> EmailVerifications => Set<EmailVerification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<TestPaper> TestPapers => Set<TestPaper>();
    public DbSet<TestSection> TestSections => Set<TestSection>();
    public DbSet<TestAttempt> TestAttempts => Set<TestAttempt>();
    public DbSet<SectionAttempt> SectionAttempts => Set<SectionAttempt>();
    public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();
    public DbSet<UserEdu> UsersEdu => Set<UserEdu>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(b);
        b.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).HasMaxLength(255).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(255);
            e.Property(x => x.FullName).HasMaxLength(255).IsRequired();
            e.Property(x => x.IsActive).HasDefaultValue(true);
        });

        b.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Code).HasMaxLength(50).IsRequired();
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        b.Entity<UserRole>(e =>
        {
            e.ToTable("user_roles");
            e.HasKey(x => new { x.UserId, x.RoleId });
            e.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
        });

        b.Entity<EmailVerification>(e =>
        {
            e.ToTable("email_verifications");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.UserId);
            e.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        });

        b.Entity<AuditLog>(e =>
        {
            e.ToTable("audit_logs");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EntityKind, x.EntityId });
            e.HasIndex(x => x.UserId);
        });

        b.Entity<LeaderboardEntry>(e =>
        {
            e.ToTable("leaderboard_entries");
            e.HasKey(x => x.Id);
        });

        b.Entity<UserEdu>(e =>
        {
            e.ToView("v_users_edu").HasNoKey();

        });
        
        b.Entity<Domain.Entities.Leaderboard.LeaderboardRow>(e =>
        {
            e.HasNoKey();
            e.ToView(null); // query từ SQL raw, không map view/table cố định
        });

    }
}
