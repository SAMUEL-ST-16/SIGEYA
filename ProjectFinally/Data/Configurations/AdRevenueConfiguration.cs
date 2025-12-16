using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class AdRevenueConfiguration : IEntityTypeConfiguration<AdRevenue>
{
    public void Configure(EntityTypeBuilder<AdRevenue> builder)
    {
        builder.HasKey(r => r.RevenueId);

        builder.Property(r => r.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(r => r.CTR)
            .HasColumnType("decimal(5,2)");

        builder.Property(r => r.CPM)
            .HasColumnType("decimal(18,4)");

        builder.Property(r => r.CPC)
            .HasColumnType("decimal(18,4)");

        builder.Property(r => r.Notes)
            .HasMaxLength(500);

        builder.HasIndex(r => r.RevenueDate);

        // Relationship with Video
        builder.HasOne(r => r.Video)
            .WithMany(v => v.AdRevenues)
            .HasForeignKey(r => r.VideoId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relationship with Campaign (configured in AdSenseCampaignConfiguration)
    }
}
