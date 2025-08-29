using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Domain.Entities.Leaderboard;
using Education.Domain.Entities;
using Edumination.Domain.Entities;

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
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();
    public DbSet<UserEdu> UsersEdu => Set<UserEdu>();
    public DbSet<PasswordReset> PasswordResets => Set<PasswordReset>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Passage> Passages => Set<Passage>();
    public DbSet<Exercise> Exercises => Set<Exercise>();



    protected override void OnModelCreating(ModelBuilder b)
    {
        b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(b);

        // Cấu hình User
        b.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();

            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(255);
            e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(255).IsRequired();
            e.Property(x => x.AvatarUrl).HasColumnName("avatar_url").HasMaxLength(500);
            e.Property(x => x.EmailVerified).HasColumnName("email_verified");
            e.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        // Cấu hình Role
        b.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Code).HasMaxLength(50).IsRequired();
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        // Cấu hình UserRole
        b.Entity<UserRole>(e =>
        {
            e.ToTable("user_roles");
            e.HasKey(x => new { x.UserId, x.RoleId });
            e.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
        });

        // Cấu hình EmailVerification
        b.Entity<EmailVerification>(e =>
        {
            e.ToTable("email_verifications");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.UserId);
            e.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        });

        // Cấu hình AuditLog
        b.Entity<AuditLog>(e =>
        {
            e.ToTable("audit_logs");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EntityKind, x.EntityId });
            e.HasIndex(x => x.UserId);
        });

        // Cấu hình LeaderboardEntry
        b.Entity<LeaderboardEntry>(e =>
        {
            e.ToTable("leaderboard_entries");
            e.HasKey(x => x.Id);
        });

        // Cấu hình UserEdu (view)
        b.Entity<UserEdu>(e =>
        {
            e.ToView("v_users_edu").HasNoKey();
        });

        // Cấu hình Asset
        b.Entity<Asset>(e =>
        {
            e.ToTable("assets");
            e.HasKey(x => x.Id);
            e.Property(x => x.Kind).HasColumnName("kind");
            e.Property(x => x.StorageUrl).HasColumnName("storage_url").HasMaxLength(1000).IsRequired();
            e.Property(x => x.MediaType).HasColumnName("media_type").HasMaxLength(100);
            e.Property(x => x.ByteSize).HasColumnName("byte_size");
            e.Property(x => x.CreatedBy).HasColumnName("created_by");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasIndex(x => x.CreatedBy).HasDatabaseName("idx_assets_creator");
        });

        b.Entity<TestPaper>(e =>
{
    e.ToTable("test_papers");
    e.HasKey(x => x.Id);
    e.Property(x => x.Code).HasColumnName("code").HasMaxLength(50);
    e.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
    e.Property(x => x.SourceType).HasColumnName("source_type").HasMaxLength(50).IsRequired();
    e.Property(x => x.UploadMethod).HasColumnName("upload_method").HasMaxLength(50).IsRequired();
    e.Property(x => x.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
    e.Property(x => x.PdfAssetId).HasColumnName("pdf_asset_id");
    e.Property(x => x.CreatedAt).HasColumnName("created_at");
    e.Property(x => x.PublishedAt).HasColumnName("published_at");
    e.Property(x => x.CreatedBy).HasColumnName("created_by"); // Thêm cấu hình này

    e.HasOne(tp => tp.PdfAsset)
     .WithMany()
     .HasForeignKey(tp => tp.PdfAssetId)
     .OnDelete(DeleteBehavior.Restrict);

    e.HasMany(tp => tp.TestSections)
     .WithOne(ts => ts.TestPaper)
     .HasForeignKey(ts => ts.PaperId);

    e.HasOne(tp => tp.CreatedByUser) // Cấu hình mối quan hệ với User
     .WithMany() // User không cần navigation ngược
     .HasForeignKey(tp => tp.CreatedBy)
     .OnDelete(DeleteBehavior.Restrict);
}); 

        // Cấu hình TestSection
        b.Entity<TestSection>(e =>
        {
            e.ToTable("test_sections");
            e.HasKey(x => x.Id);
            e.Property(x => x.PaperId).HasColumnName("paper_id");
            e.Property(x => x.Skill).HasColumnName("skill").HasMaxLength(50).IsRequired();
            e.Property(x => x.SectionNo).HasColumnName("section_no");
            e.Property(x => x.TimeLimitSec).HasColumnName("time_limit_sec");
            e.Property(x => x.IsPublished).HasColumnName("is_published");
            e.Property(x => x.AudioAssetId).HasColumnName("audio_asset_id"); // Foreign key

            // Mối quan hệ với TestPaper
            e.HasOne(ts => ts.TestPaper)
             .WithMany(tp => tp.TestSections)
             .HasForeignKey(ts => ts.PaperId)
             .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ với Asset (Audio)
            _ = e.HasOne(ts => ts.AudioAsset)
             .WithMany(a => a.TestSections)
             .HasForeignKey(ts => ts.AudioAssetId)
             .OnDelete(DeleteBehavior.Restrict); // Ngăn chặn xóa cascade nếu cần
        });

        b.Entity<PasswordReset>(e =>
        {
            e.ToTable("password_resets");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.UserId);
            e.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        });

        b.Entity<Passage>(e =>
{
    e.ToTable("passages");
    e.HasKey(x => x.Id);
    e.Property(x => x.Title).HasColumnName("title").HasMaxLength(255);
    e.Property(x => x.ContentText).HasColumnName("content_text");
    e.Property(x => x.CreatedAt).HasColumnName("created_at");

    e.HasOne(p => p.TestSection)
     .WithMany(ts => ts.Passages)
     .HasForeignKey(p => p.SectionId);

    e.HasOne(p => p.Audio)
     .WithMany()
     .HasForeignKey(p => p.AudioId);

    e.HasOne(p => p.Transcript)
     .WithMany()
     .HasForeignKey(p => p.TranscriptId);
});

        b.Entity<Question>(e =>
        {
            e.ToTable("questions");
            e.HasKey(x => x.Id);
            e.Property(x => x.Qtype).HasColumnName("qtype").HasMaxLength(50).IsRequired();
            e.Property(x => x.Stem).HasColumnName("stem").IsRequired();
            e.Property(x => x.Position).HasColumnName("position");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");

            e.HasOne(q => q.Passage)
             .WithMany(p => p.Questions)
             .HasForeignKey(q => q.PassageId);
        });

        b.Entity<Exercise>(e =>
{
    e.ToTable("exercises");
    e.HasKey(x => x.Id);

    e.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
    e.Property(x => x.Description).HasColumnName("description").HasMaxLength(2000);
    e.Property(x => x.CreatedBy).HasColumnName("created_by");
    e.Property(x => x.CreatedAt).HasColumnName("created_at");

    // Quan hệ với User
    e.HasOne(x => x.CreatedByUser)
     .WithMany()
     .HasForeignKey(x => x.CreatedBy)
     .OnDelete(DeleteBehavior.Restrict);
});

    }
}