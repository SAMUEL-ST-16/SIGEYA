using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinally.Models.Entities;

public class VideoAnalytics
{
    [Key]
    public int AnalyticsId { get; set; }

    public int ViewCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int DislikeCount { get; set; } = 0;
    public int CommentCount { get; set; } = 0;
    public int ShareCount { get; set; } = 0;

    public long WatchTimeMinutes { get; set; } = 0;

    [Column(TypeName = "decimal(5,2)")]
    public decimal? AverageViewDuration { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key (1:1 with Video)
    public int VideoId { get; set; }

    // Navigation Property
    public Video Video { get; set; } = null!;
}
