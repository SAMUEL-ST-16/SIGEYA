using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.Entities;

public class YouTubeChannel
{
    [Key]
    public int ChannelId { get; set; }

    [Required]
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

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Key - Propietario del canal
    public int? OwnerId { get; set; }

    // Navigation Properties
    public User? Owner { get; set; }
    public ICollection<Video> Videos { get; set; } = new List<Video>();
    public ICollection<AdSenseCampaign> AdSenseCampaigns { get; set; } = new List<AdSenseCampaign>();
}
