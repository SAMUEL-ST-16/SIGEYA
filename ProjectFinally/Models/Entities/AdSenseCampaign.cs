using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinally.Models.Entities;

public class AdSenseCampaign
{
    [Key]
    public int CampaignId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CampaignName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentSpent { get; set; } = 0;

    [MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active, Paused, Completed, Cancelled

    [MaxLength(100)]
    public string? AdFormat { get; set; } // Display, Video, Search, etc.

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Key
    public int ChannelId { get; set; }

    // Navigation Properties
    public YouTubeChannel Channel { get; set; } = null!;
    public ICollection<AdRevenue> AdRevenues { get; set; } = new List<AdRevenue>();
}
