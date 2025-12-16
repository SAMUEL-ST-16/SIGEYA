using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinally.Models.Entities;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required]
    [MaxLength(50)]
    public string EmployeeCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(100)]
    public string? Position { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public DateTime? HireDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Salary { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Key (1:1 with User)
    public int UserId { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;
    public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
}
