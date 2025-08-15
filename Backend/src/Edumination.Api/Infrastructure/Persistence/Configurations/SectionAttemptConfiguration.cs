using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Infrastructure.Persistence.Configurations;

public class SectionAttemptConfiguration : IEntityTypeConfiguration<SectionAttempt>
{
    public void Configure(EntityTypeBuilder<SectionAttempt> b)
    {
        b.ToTable("section_attempts");
        b.HasKey(x => x.Id);
        b.Property(x => x.TestAttemptId).HasColumnName("test_attempt_id");
        b.Property(x => x.SectionId).HasColumnName("section_id");
        b.Property(x => x.StartedAt).HasColumnName("started_at");
        b.Property(x => x.FinishedAt).HasColumnName("finished_at");
        b.Property(x => x.RawScore).HasColumnName("raw_score");
        b.Property(x => x.ScaledBand).HasColumnName("scaled_band");
        b.Property(x => x.Status).HasColumnName("status");
        b.HasIndex(x => x.SectionId).HasDatabaseName("idx_sa_section");
    }
}
