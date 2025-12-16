using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.Entities;

public class ContentSchedule
{
    [Key]
    public int ScheduleId { get; set; }

    public DateTime ScheduledDate { get; set; }

    public DateTime? PublishedDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Scheduled"; // Scheduled, Published, Cancelled, Delayed

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public bool IsRecurring { get; set; } = false;

    [MaxLength(100)]
    public string? RecurrencePattern { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Key
    public int VideoId { get; set; }

    // Navigation Property
    public Video Video { get; set; } = null!;
}
