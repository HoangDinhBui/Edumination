using Microsoft.EntityFrameworkCore;
using Edumination.Domain.Entities;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Domain.Entities.Leaderboard;
using Education.Domain.Entities;

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
    public DbSet<Passage> Passages => Set<Passage>(); // Thêm DbSet cho Passage
    public DbSet<Question> Questions => Set<Question>(); // Thêm DbSet cho Question
    public DbSet<Exercise> Exercises => Set<Exercise>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(b);

        // Cấu hình User (giữ nguyên)
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

        // Cấu hình Role (giữ nguyên)
        b.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Code).HasMaxLength(50).IsRequired();
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        // Cấu hình UserRole (giữ nguyên)
        b.Entity<UserRole>(e =>
        {
            e.ToTable("user_roles");
            e.HasKey(x => new { x.UserId, x.RoleId });
            e.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
        });

        // Cấu hình EmailVerification (giữ nguyên)
        b.Entity<EmailVerification>(e =>
        {
            e.ToTable("email_verifications");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.UserId);
            e.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        });

        // Cấu hình AuditLog (giữ nguyên)
        b.Entity<AuditLog>(e =>
        {
            e.ToTable("audit_logs");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EntityKind, x.EntityId });
            e.HasIndex(x => x.UserId);
        });

        // Cấu hình LeaderboardEntry (giữ nguyên)
        b.Entity<LeaderboardEntry>(e =>
        {
            e.ToTable("leaderboard_entries");
            e.HasKey(x => x.Id);
        });

        // Cấu hình UserEdu (giữ nguyên)
        b.Entity<UserEdu>(e =>
        {
            e.ToView("v_users_edu").HasNoKey();
        });

        // Cấu hình Asset (giữ nguyên)
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

        // Cấu hình TestPaper (giữ nguyên)
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
            e.Property(x => x.CreatedBy).HasColumnName("created_by");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.PublishedAt).HasColumnName("published_at");

            e.HasOne(tp => tp.PdfAsset)
             .WithMany()
             .HasForeignKey(tp => tp.PdfAssetId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(tp => tp.CreatedByUser)
             .WithMany()
             .HasForeignKey(tp => tp.CreatedBy)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(tp => tp.TestSections)
             .WithOne(ts => ts.TestPaper)
             .HasForeignKey(ts => ts.PaperId);
        });

        // Cấu hình TestSection (giữ nguyên)
        b.Entity<TestSection>(e =>
        {
            e.ToTable("test_sections");
            e.HasKey(x => x.Id);
            e.Property(x => x.PaperId).HasColumnName("paper_id");
            e.Property(x => x.Skill).HasColumnName("skill").HasMaxLength(50).IsRequired();
            e.Property(x => x.SectionNo).HasColumnName("section_no");
            e.Property(x => x.TimeLimitSec).HasColumnName("time_limit_sec");
            e.Property(x => x.IsPublished).HasColumnName("is_published");
            e.Property(x => x.AudioAssetId).HasColumnName("audio_asset_id");

            e.HasOne(ts => ts.TestPaper)
             .WithMany(tp => tp.TestSections)
             .HasForeignKey(ts => ts.PaperId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ts => ts.AudioAsset)
             .WithMany(a => a.TestSections)
             .HasForeignKey(ts => ts.AudioAssetId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Cấu hình Passage
        b.Entity<Passage>(e =>
        {
            e.ToTable("passages");
            e.HasKey(x => x.Id);
            e.Property(x => x.SectionId).HasColumnName("section_id");
            e.Property(x => x.Title).HasColumnName("title").HasMaxLength(255);
            e.Property(x => x.ContentText).HasColumnName("content_text");
            e.Property(x => x.AudioId).HasColumnName("audio_id");
            e.Property(x => x.TranscriptId).HasColumnName("transcript_id");
            e.Property(x => x.Position).HasColumnName("position");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");

            e.HasOne(p => p.TestSection)
             .WithMany(ts => ts.Passages)
             .HasForeignKey(p => p.SectionId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.Audio)
             .WithMany()
             .HasForeignKey(p => p.AudioId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Transcript)
             .WithMany()
             .HasForeignKey(p => p.TranscriptId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Cấu hình Question
        b.Entity<Question>(e =>
        {
            e.ToTable("questions");
            e.HasKey(x => x.Id);
            e.Property(x => x.PassageId).HasColumnName("passage_id");
            e.Property(x => x.Qtype).HasColumnName("qtype").HasMaxLength(50).IsRequired();
            e.Property(x => x.Stem).HasColumnName("stem").IsRequired();
            e.Property(x => x.MetaJson).HasColumnName("meta_json");
            e.Property(x => x.Position).HasColumnName("position");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");

            e.HasOne(q => q.Passage)
             .WithMany(p => p.Questions)
             .HasForeignKey(q => q.PassageId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}