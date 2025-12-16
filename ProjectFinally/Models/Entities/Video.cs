using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.Entities;

public class Video
{
    [Key]
    public int VideoId { get; set; }

    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    [MaxLength(100)]
    public string? YouTubeVideoId { get; set; }

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    public int? DurationSeconds { get; set; }

    public DateTime? PublishedAt { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Draft"; // Draft, Published, Unlisted, Private

    [MaxLength(500)]
    public string? Tags { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int ChannelId { get; set; }
    public int? CategoryId { get; set; }

    // Navigation Properties
    public YouTubeChannel Channel { get; set; } = null!;
    public VideoCategory? Category { get; set; }
    public VideoAnalytics? Analytics { get; set; }
    public ICollection<ContentSchedule> ContentSchedules { get; set; } = new List<ContentSchedule>();
    public ICollection<AdRevenue> AdRevenues { get; set; } = new List<AdRevenue>();
}
