using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Foreign Key
    public int RoleId { get; set; }

    // Navigation Properties
    public Role Role { get; set; } = null!;
    public Employee? Employee { get; set; } // 1:1 relationship
    public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
    public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
}
