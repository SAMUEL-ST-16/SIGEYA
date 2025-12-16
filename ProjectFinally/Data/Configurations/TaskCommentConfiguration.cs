using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasKey(c => c.CommentId);

        builder.Property(c => c.Comment)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasIndex(c => c.TaskId);
        builder.HasIndex(c => c.CreatedAt);

        // Relationship with Task (configured in TaskConfiguration)

        // Relationship with User
        builder.HasOne(c => c.User)
            .WithMany(u => u.TaskComments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
