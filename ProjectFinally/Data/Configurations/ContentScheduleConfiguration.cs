using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class ContentScheduleConfiguration : IEntityTypeConfiguration<ContentSchedule>
{
    public void Configure(EntityTypeBuilder<ContentSchedule> builder)
    {
        builder.HasKey(s => s.ScheduleId);

        builder.Property(s => s.Status)
            .HasMaxLength(50)
            .HasDefaultValue("Scheduled");

        builder.Property(s => s.Notes)
            .HasMaxLength(1000);

        builder.Property(s => s.RecurrencePattern)
            .HasMaxLength(100);

        builder.HasIndex(s => s.ScheduledDate);

        // Relationship with Video (configured in VideoConfiguration)
    }
}
