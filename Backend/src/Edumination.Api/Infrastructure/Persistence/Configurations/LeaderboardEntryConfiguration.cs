using Edumination.Api.Domain.Entities.Leaderboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edumination.Api.Infrastructure.Persistence.Configurations;

public class LeaderboardEntryConfiguration : IEntityTypeConfiguration<LeaderboardEntry>
{
    public void Configure(EntityTypeBuilder<LeaderboardEntry> b)
    {
        b.ToTable("leaderboard_entries");
        b.HasKey(x => x.Id);
        b.Property(x => x.UserId).HasColumnName("user_id");
        b.Property(x => x.PaperId).HasColumnName("paper_id");
        b.Property(x => x.BestOverallBand).HasColumnName("best_overall_band");
        b.Property(x => x.BestAt).HasColumnName("best_at");
        b.HasIndex(x => new { x.PaperId, x.BestOverallBand });
    }
}
