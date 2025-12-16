using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class VideoAnalyticsConfiguration : IEntityTypeConfiguration<VideoAnalytics>
{
    public void Configure(EntityTypeBuilder<VideoAnalytics> builder)
    {
        builder.HasKey(a => a.AnalyticsId);

        builder.Property(a => a.AverageViewDuration)
            .HasColumnType("decimal(5,2)");

        // 1:1 relationship with Video (configured in VideoConfiguration)
        builder.HasIndex(a => a.VideoId)
            .IsUnique();
    }
}
