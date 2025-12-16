using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;
using TaskEntity = ProjectFinally.Models.Entities.Task;

namespace ProjectFinally.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(t => t.TaskId);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(2000);

        builder.Property(t => t.Priority)
            .HasMaxLength(50)
            .HasDefaultValue("Medium");

        builder.Property(t => t.Status)
            .HasMaxLength(50)
            .HasDefaultValue("Pending");

        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.DueDate);

        // Relationship with User (created by)
        builder.HasOne(t => t.CreatedByUser)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Employee (assigned to)
        builder.HasOne(t => t.AssignedToEmployee)
            .WithMany(e => e.AssignedTasks)
            .HasForeignKey(t => t.AssignedToEmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relationship with TaskComments
        builder.HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
