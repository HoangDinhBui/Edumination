using Microsoft.EntityFrameworkCore;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Domain.Entities.Leaderboard;
using Education.Domain.Entities;
using Edumination.Domain.Entities;
using Edumination.Api.Domain.Entities.EduDomain;

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

    public DbSet<QuestionChoice> QuestionChoices => Set<QuestionChoice>();
    public DbSet<QuestionAnswerKey> QuestionAnswerKeys => Set<QuestionAnswerKey>();

    public DbSet<OAuthAccounts> OAuthAccounts => Set<OAuthAccounts>();
    public DbSet<OAuthStates> OAuthStates => Set<OAuthStates>();
    public DbSet<EduDomain> EduDomains => Set<EduDomain>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<LessonCompletion> LessonCompletions => Set<LessonCompletion>();
    public DbSet<UserStats> UserStats => Set<UserStats>();
    public DbSet<BandScale> BandScales { get; set; }
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<SpeakingSubmission> SpeakingSubmissions => Set<SpeakingSubmission>();
    public DbSet<WritingSubmission> WritingSubmissions => Set<WritingSubmission>();
    public DbSet<vTestAttemptBand> TestAttemptBands => Set<vTestAttemptBand>();
    public DbSet<CoursePrice> CoursePrices => Set<CoursePrice>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Edumination.Api.Domain.Entities.MockTest> MockTests => Set<Edumination.Api.Domain.Entities.MockTest>();
    public DbSet<Edumination.Api.Domain.Entities.MockTestQuarter> MockTestQuarters => Set<Edumination.Api.Domain.Entities.MockTestQuarter>();

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
            e.HasMany(u => u.TestAttempts).WithOne(ta => ta.User).HasForeignKey(ta => ta.UserId);
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
            e.Property(x => x.DurationSec).HasColumnName("duration_sec");
            e.Property(x => x.Sha256).HasColumnName("sha256").HasMaxLength(64);
            e.Property(x => x.LanguageCode).HasColumnName("language_code").HasMaxLength(10);
            e.Property(x => x.CreatedBy).HasColumnName("created_by");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasIndex(x => x.CreatedBy).HasDatabaseName("idx_assets_creator");
        });

        b.Entity<SpeakingSubmission>(e =>
        {
            e.ToTable("speaking_submissions");
            e.HasKey(x => x.Id);
            e.Property(x => x.SectionAttemptId).HasColumnName("section_attempt_id").IsRequired();
            e.Property(x => x.PromptText).HasColumnName("prompt_text");
            e.Property(x => x.AudioAssetId).HasColumnName("audio_asset_id").IsRequired();
            e.Property(x => x.AsrText).HasColumnName("asr_text");
            e.Property(x => x.WordsCount).HasColumnName("words_count");
            e.Property(x => x.DurationSec).HasColumnName("duration_sec");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasOne(s => s.SectionAttempt)
                .WithOne()
                .HasForeignKey<SpeakingSubmission>(s => s.SectionAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(s => s.AudioAsset)
                .WithMany()
                .HasForeignKey(s => s.AudioAssetId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(s => s.SectionAttemptId).IsUnique();
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

            e.HasMany(tp => tp.TestSections).WithOne(ts => ts.TestPaper).HasForeignKey(ts => ts.PaperId);
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
            e.Property(x => x.AudioAssetId).HasColumnName("audio_asset_id");

            e.HasOne(ts => ts.TestPaper)
                .WithMany(tp => tp.TestSections)
                .HasForeignKey(ts => ts.PaperId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ts => ts.AudioAsset)
                .WithMany(a => a.TestSections)
                .HasForeignKey(ts => ts.AudioAssetId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(s => new { s.PaperId, s.Skill }).IsUnique(); // Thêm chỉ mục duy nhất
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
            e.Property(x => x.SectionId).HasColumnName("section_id").IsRequired();
            e.Property(x => x.Title).HasColumnName("title").HasMaxLength(255);
            e.Property(x => x.ContentText).HasColumnName("content_text");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");

            e.HasOne(p => p.TestSection)
                .WithMany(ts => ts.Passages)
                .HasForeignKey(p => p.SectionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            e.HasOne(p => p.Audio)
                .WithMany()
                .HasForeignKey(p => p.AudioId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Transcript)
                .WithMany()
                .HasForeignKey(p => p.TranscriptId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(pa => pa.Questions).WithOne(q => q.Passage).HasForeignKey(q => q.PassageId);
        });

        b.Entity<Question>(e =>
        {
            e.ToTable("questions");
            e.HasKey(x => x.Id);
            e.Property(x => x.SectionId).HasColumnName("section_id").IsRequired();
            e.Property(x => x.PassageId).HasColumnName("passage_id"); // Loại bỏ .IsRequired() nếu nullable
            e.Property(x => x.Qtype).HasColumnName("qtype").HasMaxLength(50).IsRequired(); // Thêm .IsRequired() nếu bắt buộc
            e.Property(x => x.Stem).HasColumnName("stem").IsRequired(); // Thêm .IsRequired() nếu bắt buộc
            e.Property(x => x.Position).HasColumnName("position");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasOne(q => q.Passage)
                .WithMany(p => p.Questions)
                .HasForeignKey(q => q.PassageId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(q => q.QuestionChoices)
                .WithOne(c => c.Question)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(q => q.QuestionAnswerKey)
                .WithOne(k => k.Question)
                .HasForeignKey<QuestionAnswerKey>(k => k.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
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

        b.Entity<QuestionChoice>(e =>
        {
            e.ToTable("question_choices");
            e.HasKey(x => x.Id);
            e.Property(x => x.QuestionId).HasColumnName("question_id");
            e.Property(x => x.Content).HasColumnName("content").IsRequired();
            e.Property(x => x.IsCorrect).HasColumnName("is_correct");
            e.Property(x => x.Position).HasColumnName("position");

            e.HasOne(qc => qc.Question)
                .WithMany(q => q.QuestionChoices)
                .HasForeignKey(qc => qc.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<QuestionAnswerKey>(e =>
        {
            e.ToTable("question_answer_keys");
            e.HasKey(x => x.Id);
            e.Property(x => x.QuestionId).HasColumnName("question_id");
            e.Property(x => x.KeyJson).HasColumnName("key_json").IsRequired();

            e.HasOne(qak => qak.Question)
                .WithOne(q => q.QuestionAnswerKey)
                .HasForeignKey<QuestionAnswerKey>(qak => qak.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<OAuthAccounts>(e =>
        {
            e.ToTable("oauth_accounts");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.Provider, x.ProviderUserId }).IsUnique();
            e.HasIndex(x => new { x.UserId, x.Provider }).IsUnique();
            e.Property(x => x.Provider).HasMaxLength(50).IsRequired();
            e.Property(x => x.ProviderUserId).HasMaxLength(128).IsRequired();
            e.Property(x => x.Email).HasMaxLength(255).IsRequired();
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        });

        b.Entity<OAuthStates>(e =>
        {
            e.ToTable("oauth_states");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.Provider, x.State }).IsUnique();
            e.Property(x => x.Provider).HasMaxLength(50).IsRequired();
            e.Property(x => x.State).HasMaxLength(128).IsRequired();
        });

        b.Entity<EduDomain>(e =>
        {
            e.ToTable("edu_domains");
            e.HasKey(x => x.Id);
            e.Property(x => x.Domain).HasMaxLength(255).IsRequired();
            e.HasIndex(x => x.Domain).IsUnique();
        });

        b.Entity<Course>(e =>
        {
            e.ToTable("courses");
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(255).IsRequired();
            e.Property(x => x.Description).HasColumnType("text");
            e.Property(x => x.Level).HasConversion<string>().HasMaxLength(32).HasColumnName("level");
            e.Property(x => x.IsPublished).HasColumnName("is_published");
            e.Property(x => x.CreatedBy).HasColumnName("created_by");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        });

        b.Entity<Enrollment>(e =>
        {
            e.ToTable("enrollments");
            e.HasKey(x => new { x.UserId, x.CourseId });
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.CourseId).HasColumnName("course_id");
            e.Property(x => x.EnrolledAt).HasColumnName("enrolled_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Course).WithMany(c => c.Enrollments).HasForeignKey(x => x.CourseId);
        });

        // ===== Modules =====
        b.Entity<Module>(e =>
        {
            e.ToTable("modules");
            e.HasKey(x => x.Id);

            e.Property(x => x.Title).HasMaxLength(255).IsRequired();
            e.Property(x => x.Description);
            e.Property(x => x.Position).IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasIndex(x => new { x.CourseId, x.Position }).IsUnique();
        });

        // ===== Lessons =====
        b.Entity<Lesson>(e =>
        {
            e.ToTable("lessons");
            e.HasKey(x => x.Id);

            e.Property(x => x.Title).HasMaxLength(255).IsRequired();
            e.Property(x => x.Objective);
            e.Property(x => x.Position).IsRequired();
            e.Property(x => x.IsPublished).HasDefaultValue(false);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasIndex(x => new { x.ModuleId, x.Position }).IsUnique();
        });

        // ===== LessonCompletions =====
        b.Entity<LessonCompletion>(e =>
        {
            e.ToTable("lesson_completions");
            e.HasKey(x => new { x.UserId, x.LessonId });

            e.Property(x => x.CompletedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        });

        // ===== UserStats =====
        b.Entity<UserStats>(e =>
        {
            e.ToTable("user_stats");
            e.HasKey(x => x.UserId);
            e.Property(x => x.BestBand).HasColumnType("decimal(4,2)");
            e.Property(e => e.BestSkill)
                .HasConversion<string>(); // chuyển enum ↔ string

            e.Property(e => e.WorstSkill)
                .HasConversion<string>();
            e.Property(x => x.TotalTests);
            e.Property(x => x.UpdatedAt);
        });

        b.Entity<BandScale>(e =>
        {
            e.ToTable("band_scales");
            e.HasKey(x => x.Id);
            e.Property(x => x.PaperId).HasColumnName("paper_id");
            e.Property(x => x.Skill).HasColumnName("skill").HasMaxLength(50).IsRequired();
            e.Property(x => x.RawMin).HasColumnName("raw_min");
            e.Property(x => x.RawMax).HasColumnName("raw_max");
            e.Property(x => x.Band).HasColumnName("band").HasColumnType("decimal(3,1)");

            e.HasOne(bs => bs.TestPaper)
                .WithMany(tp => tp.BandScales)
                .HasForeignKey(bs => bs.PaperId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(bs => new { bs.PaperId, bs.Skill, bs.RawMin }).IsUnique();
        });

        // Cấu hình TestAttempt
        b.Entity<TestAttempt>(e =>
        {
            e.ToTable("test_attempts");
            e.HasKey(x => x.Id);
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.PaperId).HasColumnName("paper_id");
            e.Property(x => x.AttemptNo).HasColumnName("attempt_no");
            e.Property(x => x.StartedAt).HasColumnName("started_at");
            e.Property(x => x.FinishedAt).HasColumnName("finished_at");
            e.Property(x => x.Status).HasColumnName("status").HasMaxLength(50).HasDefaultValue("IN_PROGRESS");

            e.HasOne(ta => ta.User)
                .WithMany(u => u.TestAttempts)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(ta => ta.TestPaper)
                .WithMany(tp => tp.TestAttempts)
                .HasForeignKey(ta => ta.PaperId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(ta => ta.SectionAttempts)
                .WithOne(sa => sa.TestAttempt)
                .HasForeignKey(sa => sa.TestAttemptId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Cấu hình SectionAttempt
        b.Entity<SectionAttempt>(e =>
        {
            e.ToTable("section_attempts");
            e.HasKey(x => x.Id);
            e.Property(x => x.TestAttemptId).HasColumnName("test_attempt_id");
            e.Property(x => x.SectionId).HasColumnName("section_id");
            e.Property(x => x.StartedAt).HasColumnName("started_at");
            e.Property(x => x.FinishedAt).HasColumnName("finished_at");
            e.Property(x => x.RawScore).HasColumnName("raw_score").HasColumnType("decimal(10,2)");
            e.Property(x => x.ScaledBand).HasColumnName("scaled_band").HasColumnType("decimal(4,2)");
            e.Property(x => x.Status).HasColumnName("status").HasMaxLength(50).HasDefaultValue("IN_PROGRESS");

            e.HasOne(sa => sa.TestAttempt)
                .WithMany(ta => ta.SectionAttempts)
                .HasForeignKey(sa => sa.TestAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(sa => sa.TestSection)
                .WithMany(ts => ts.SectionAttempts)
                .HasForeignKey(sa => sa.SectionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(sa => sa.Answers)
                .WithOne(a => a.SectionAttempt)
                .HasForeignKey(a => a.SectionAttemptId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Cấu hình Answer
        b.Entity<Answer>(e =>
        {
            e.ToTable("answers");
            e.HasKey(a => a.Id);
            e.Property(a => a.SectionAttemptId).IsRequired();
            e.Property(a => a.QuestionId).IsRequired();
            e.Property(a => a.AnswerJson).IsRequired();
            e.Property(a => a.AnswerJson).HasColumnType("json");
            e.Property(a => a.IsCorrect);
            e.Property(a => a.EarnedScore).HasColumnType("decimal(6,2)");
            e.Property(a => a.CheckedAt);

            e.HasOne(a => a.SectionAttempt)
                .WithMany(sa => sa.Answers)
                .HasForeignKey(a => a.SectionAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(a => new { a.SectionAttemptId, a.QuestionId }).IsUnique();
        });

        // Cấu hình WritingSubmission
        b.Entity<WritingSubmission>(e =>
        {
            e.ToTable("writing_submissions");
            e.HasKey(x => x.Id);
            e.Property(x => x.SectionAttemptId).HasColumnName("section_attempt_id").IsRequired();
            e.Property(x => x.ContentText).HasColumnName("content_text").IsRequired();
            e.Property(x => x.PromptText).HasColumnName("prompt_text");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.WordsCount).HasColumnName("words_count").IsRequired();

            e.HasOne(s => s.SectionAttempt)
                .WithOne()
                .HasForeignKey<WritingSubmission>(s => s.SectionAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(s => s.SectionAttemptId).IsUnique();
        });

        b.Entity<vTestAttemptBand>(e =>
        {
            e.ToView("v_test_attempt_band").HasNoKey();
            e.Property(x => x.TestAttemptId).HasColumnName("test_attempt_id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.PaperId).HasColumnName("paper_id");
            e.Property(x => x.OverallBand).HasColumnName("overall_band").HasColumnType("decimal(3,1)");
        });

        b.Entity<CoursePrice>(e =>
        {
            e.ToTable("course_prices");
            e.HasKey(x => x.CourseId);
            e.Property(x => x.PriceVnd).HasColumnName("price_vnd");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasOne(cp => cp.Course)
            .WithOne()
            .HasForeignKey<CoursePrice>(cp => cp.CourseId);
        });

        b.Entity<Order>(e =>
        {
            e.ToTable("orders");
            e.HasKey(x => x.Id);
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.TotalVnd).HasColumnName("total_vnd");
            e.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.PaidAt).HasColumnName("paid_at");
            e.HasMany(x => x.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<OrderItem>(e =>
        {
            e.ToTable("order_items");
            e.HasKey(x => x.Id);
            e.Property(x => x.OrderId).HasColumnName("order_id");
            e.Property(x => x.CourseId).HasColumnName("course_id");
            e.Property(x => x.UnitVnd).HasColumnName("unit_vnd");
            e.Property(x => x.Qty).HasColumnName("qty");
            e.HasIndex(x => new { x.OrderId, x.CourseId })
            .IsUnique()
            .HasDatabaseName("uq_order_course");
        });

        b.Entity<Payment>(e =>
        {
            e.ToTable("payments");
            e.HasKey(x => x.Id);
            e.Property(x => x.OrderId).HasColumnName("order_id");
            e.Property(x => x.Provider)
                .HasColumnName("provider")
                .HasConversion<string>();
            e.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>();
            e.Property(x => x.ProviderTxnId).HasColumnName("provider_txn_id");
            e.Property(x => x.AmountVnd).HasColumnName("amount_vnd");
            e.Property(x => x.RawResponse)
                .HasColumnName("raw_response")
                .HasColumnType("json");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        b.Entity<Edumination.Api.Domain.Entities.MockTest>(e =>
    {
        e.ToTable("mock_tests");
        e.HasKey(x => x.Id);
        e.Property(x => x.Title).HasMaxLength(255).IsRequired();
        e.Property(x => x.Year).IsRequired();
        e.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);

        // Quan hệ 1 MockTest có nhiều MockTestQuarter
        e.HasMany(mt => mt.Quarters)
           .WithOne(mtq => mtq.MockTest)
           .HasForeignKey(mtq => mtq.MockTestId);
    });

        b.Entity<MockTestQuarter>(e =>
        {
            e.ToTable("mock_test_quarters");
            e.HasKey(x => x.Id);
            e.Property(x => x.MockTestId).HasColumnName("mock_test_id");
            e.Property(x => x.Quarter).HasConversion<string>().HasMaxLength(10); // Giả sử là Enum Q1, Q2...
            e.Property(x => x.SetNumber).HasColumnName("set_number");
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);

            // Cấu hình các Foreign Key đến TestPaper (nếu cần)
            e.HasOne(mtq => mtq.ListeningPaper)
               .WithMany()
               .HasForeignKey(mtq => mtq.ListeningPaperId)
               .OnDelete(DeleteBehavior.Restrict); // Hoặc SetNull tùy logic

            e.HasOne(mtq => mtq.ReadingPaper)
               .WithMany()
               .HasForeignKey(mtq => mtq.ReadingPaperId)
               .OnDelete(DeleteBehavior.Restrict);

            // (Tương tự cho WritingPaperId và SpeakingPaperId)
        });
    }
}