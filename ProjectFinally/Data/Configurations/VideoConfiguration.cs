using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasKey(v => v.VideoId);

        builder.Property(v => v.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasIndex(v => v.Title);

        builder.Property(v => v.Description)
            .HasMaxLength(2000);

        builder.Property(v => v.VideoUrl)
            .HasMaxLength(500);

        builder.Property(v => v.YouTubeVideoId)
            .HasMaxLength(100);

        builder.HasIndex(v => v.YouTubeVideoId)
            .IsUnique();

        builder.Property(v => v.ThumbnailUrl)
            .HasMaxLength(500);

        builder.Property(v => v.Status)
            .HasMaxLength(50)
            .HasDefaultValue("Draft");

        builder.Property(v => v.Tags)
            .HasMaxLength(500);

        // Relationship with Channel (configured in YouTubeChannelConfiguration)

        // Relationship with Category (configured in VideoCategoryConfiguration)

        // 1:1 relationship with VideoAnalytics
        builder.HasOne(v => v.Analytics)
            .WithOne(a => a.Video)
            .HasForeignKey<VideoAnalytics>(a => a.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:N relationship with ContentSchedules
        builder.HasMany(v => v.ContentSchedules)
            .WithOne(s => s.Video)
            .HasForeignKey(s => s.VideoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
