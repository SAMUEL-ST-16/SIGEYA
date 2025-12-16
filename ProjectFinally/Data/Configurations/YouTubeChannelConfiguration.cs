using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class YouTubeChannelConfiguration : IEntityTypeConfiguration<YouTubeChannel>
{
    public void Configure(EntityTypeBuilder<YouTubeChannel> builder)
    {
        builder.HasKey(c => c.ChannelId);

        builder.Property(c => c.ChannelName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.ChannelName);

        builder.Property(c => c.ChannelUrl)
            .HasMaxLength(500);

        builder.Property(c => c.YouTubeChannelId)
            .HasMaxLength(100);

        builder.HasIndex(c => c.YouTubeChannelId)
            .IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        // 1:N relationship with Videos
        builder.HasMany(c => c.Videos)
            .WithOne(v => v.Channel)
            .HasForeignKey(v => v.ChannelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
