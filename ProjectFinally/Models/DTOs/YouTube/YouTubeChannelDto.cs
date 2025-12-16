using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.YouTube;

public class YouTubeChannelDto
{
    public int ChannelId { get; set; }
    public string ChannelName { get; set; } = string.Empty;
    public string? ChannelUrl { get; set; }
    public string? YouTubeChannelId { get; set; }
    public string? Description { get; set; }
    public int SubscriberCount { get; set; }
    public DateTime? CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? OwnerId { get; set; }
    public int TotalVideos { get; set; }
}

public class CreateYouTubeChannelDto
{
    [Required(ErrorMessage = "Channel name is required")]
    [MaxLength(200)]
    public string ChannelName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ChannelUrl { get; set; }

    [MaxLength(100)]
    public string? YouTubeChannelId { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int SubscriberCount { get; set; } = 0;

    public DateTime? CreatedDate { get; set; }
}

public class UpdateYouTubeChannelDto
{
    [Required(ErrorMessage = "Channel name is required")]
    [MaxLength(200)]
    public string ChannelName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ChannelUrl { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int SubscriberCount { get; set; }

    public bool IsActive { get; set; }
}
