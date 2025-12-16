namespace ProjectFinally.Models.DTOs.YouTube;

public class VideoDto
{
    public int VideoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public string? YouTubeVideoId { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int? DurationSeconds { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Tags { get; set; }
    public DateTime CreatedAt { get; set; }

    public int ChannelId { get; set; }
    public string ChannelName { get; set; } = string.Empty;
    public int? ChannelOwnerId { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public VideoAnalyticsDto? Analytics { get; set; }
}

public class VideoAnalyticsDto
{
    public int AnalyticsId { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public int CommentCount { get; set; }
    public int ShareCount { get; set; }
    public long WatchTimeMinutes { get; set; }
    public decimal? AverageViewDuration { get; set; }
    public DateTime LastUpdated { get; set; }
}
