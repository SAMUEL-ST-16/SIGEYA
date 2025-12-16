using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.Tasks;

public class TaskDto
{
    public int TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Creator info
    public int CreatedByUserId { get; set; }
    public string CreatedByUsername { get; set; } = string.Empty;
    public string CreatedByFullName { get; set; } = string.Empty;

    // Assignee info
    public int? AssignedToEmployeeId { get; set; }
    public string? AssignedToEmployeeName { get; set; }
    public string? AssignedToEmployeeCode { get; set; }

    // Calculated
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != "Completed";
    public int DaysUntilDue => DueDate.HasValue
        ? (DueDate.Value - DateTime.UtcNow).Days
        : 0;
}

public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Priority is required")]
    [MaxLength(50)]
    [RegularExpression("^(Low|Medium|High|Urgent)$", ErrorMessage = "Priority must be: Low, Medium, High, or Urgent")]
    public string Priority { get; set; } = "Medium";

    public DateTime? DueDate { get; set; }

    public int? AssignedToEmployeeId { get; set; }
}

public class UpdateTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    [RegularExpression("^(Low|Medium|High|Urgent)$", ErrorMessage = "Priority must be: Low, Medium, High, or Urgent")]
    public string Priority { get; set; } = "Medium";

    [MaxLength(50)]
    [RegularExpression("^(Pending|InProgress|Completed|Cancelled)$", ErrorMessage = "Status must be: Pending, InProgress, Completed, or Cancelled")]
    public string Status { get; set; } = "Pending";

    public DateTime? DueDate { get; set; }

    public int? AssignedToEmployeeId { get; set; }
}

public class TaskCommentDto
{
    public int CommentId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Task info
    public int TaskId { get; set; }
    public string TaskTitle { get; set; } = string.Empty;

    // User info
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
}

public class CreateTaskCommentDto
{
    [Required(ErrorMessage = "Comment is required")]
    [MaxLength(2000)]
    public string Comment { get; set; } = string.Empty;

    [Required(ErrorMessage = "Task ID is required")]
    public int TaskId { get; set; }
}
