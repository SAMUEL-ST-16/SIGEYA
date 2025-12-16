using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.EmployeeId);

        builder.Property(e => e.EmployeeCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.EmployeeCode)
            .IsUnique();

        builder.Property(e => e.Department)
            .HasMaxLength(100);

        builder.Property(e => e.Position)
            .HasMaxLength(100);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.Salary)
            .HasColumnType("decimal(18,2)");

        // 1:1 relationship with User (already configured in UserConfiguration)

        // 1:N relationship with Tasks will be added in Phase 4
    }
}
