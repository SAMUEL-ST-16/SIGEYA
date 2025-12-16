using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.AdSense;

public class AdSenseCampaignDto
{
    public int CampaignId { get; set; }
    public string CampaignName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Budget { get; set; }
    public decimal CurrentSpent { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AdFormat { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Channel info
    public int ChannelId { get; set; }
    public string ChannelName { get; set; } = string.Empty;
    public int? ChannelOwnerId { get; set; }

    // Calculated
    public decimal RemainingBudget => Budget - CurrentSpent;
    public int DaysRunning => EndDate.HasValue
        ? (DateTime.UtcNow - StartDate).Days
        : (DateTime.UtcNow - StartDate).Days;
}

public class CreateAdSenseCampaignDto
{
    [Required(ErrorMessage = "Campaign name is required")]
    [MaxLength(200)]
    public string CampaignName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Budget is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Budget must be greater than 0")]
    public decimal Budget { get; set; }

    [MaxLength(100)]
    public string? AdFormat { get; set; }

    [Required(ErrorMessage = "Channel ID is required")]
    public int ChannelId { get; set; }
}

public class UpdateAdSenseCampaignDto
{
    [Required(ErrorMessage = "Campaign name is required")]
    [MaxLength(200)]
    public string CampaignName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime? EndDate { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Budget must be greater than 0")]
    public decimal Budget { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    [MaxLength(100)]
    public string? AdFormat { get; set; }

    public bool IsActive { get; set; }
}
