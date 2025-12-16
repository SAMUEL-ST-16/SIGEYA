using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class AdSenseCampaignConfiguration : IEntityTypeConfiguration<AdSenseCampaign>
{
    public void Configure(EntityTypeBuilder<AdSenseCampaign> builder)
    {
        builder.HasKey(c => c.CampaignId);

        builder.Property(c => c.CampaignName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.CampaignName);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Status)
            .HasMaxLength(50)
            .HasDefaultValue("Active");

        builder.Property(c => c.Budget)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.CurrentSpent)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.AdFormat)
            .HasMaxLength(100);

        // Relationship with YouTubeChannel
        builder.HasOne(c => c.Channel)
            .WithMany(ch => ch.AdSenseCampaigns)
            .HasForeignKey(c => c.ChannelId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with AdRevenues
        builder.HasMany(c => c.AdRevenues)
            .WithOne(r => r.Campaign)
            .HasForeignKey(r => r.CampaignId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
