using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.Entities;

public class Task
{
    [Key]
    public int TaskId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string Priority { get; set; } = "Medium"; // Low, Medium, High, Urgent

    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled

    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int CreatedByUserId { get; set; }
    public int? AssignedToEmployeeId { get; set; }

    // Navigation Properties
    public User CreatedByUser { get; set; } = null!;
    public Employee? AssignedToEmployee { get; set; }
    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
}
