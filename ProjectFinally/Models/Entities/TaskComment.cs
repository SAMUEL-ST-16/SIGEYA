using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.Entities;

public class TaskComment
{
    [Key]
    public int CommentId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int TaskId { get; set; }
    public int UserId { get; set; }

    // Navigation Properties
    public Task Task { get; set; } = null!;
    public User User { get; set; } = null!;
}
