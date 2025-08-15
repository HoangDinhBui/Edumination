using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Infrastructure.Persistence.Configurations;

public class TestAttemptConfiguration : IEntityTypeConfiguration<TestAttempt>
{
    public void Configure(EntityTypeBuilder<TestAttempt> b)
    {
        b.ToTable("test_attempts");
        b.HasKey(x => x.Id);
        b.Property(x => x.UserId).HasColumnName("user_id");
        b.Property(x => x.PaperId).HasColumnName("paper_id");
        b.Property(x => x.AttemptNo).HasColumnName("attempt_no");
        b.Property(x => x.StartedAt).HasColumnName("started_at");
        b.Property(x => x.FinishedAt).HasColumnName("finished_at");
        b.Property(x => x.Status).HasColumnName("status");
        b.HasIndex(x => x.UserId).HasDatabaseName("idx_ta_user");
        b.HasIndex(x => x.PaperId).HasDatabaseName("idx_ta_paper");
    }
}
