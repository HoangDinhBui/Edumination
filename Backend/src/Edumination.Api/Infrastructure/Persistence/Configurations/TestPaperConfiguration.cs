using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Infrastructure.Persistence.Configurations;

public class TestPaperConfiguration : IEntityTypeConfiguration<TestPaper>
{
    public void Configure(EntityTypeBuilder<TestPaper> b)
    {
        b.ToTable("test_papers");
        b.HasKey(x => x.Id);
        b.Property(x => x.Code).HasColumnName("code");
        b.Property(x => x.Title).HasColumnName("title").IsRequired();
        b.Property(x => x.SourceType).HasColumnName("source_type");
        b.Property(x => x.UploadMethod).HasColumnName("upload_method");
        b.Property(x => x.Status).HasColumnName("status");
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.PublishedAt).HasColumnName("published_at");
    }
}
