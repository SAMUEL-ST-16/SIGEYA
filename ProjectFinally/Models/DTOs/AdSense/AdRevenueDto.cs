using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.AdSense;

public class AdRevenueDto
{
    public int RevenueId { get; set; }
    public DateTime RevenueDate { get; set; }
    public decimal Amount { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public decimal? CTR { get; set; }
    public decimal? CPM { get; set; }
    public decimal? CPC { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Video info
    public int? VideoId { get; set; }
    public string? VideoTitle { get; set; }

    // Campaign info
    public int? CampaignId { get; set; }
    public string? CampaignName { get; set; }
}

public class CreateAdRevenueDto
{
    [Required(ErrorMessage = "Revenue date is required")]
    public DateTime RevenueDate { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Amount must be 0 or greater")]
    public decimal Amount { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Impressions must be 0 or greater")]
    public int Impressions { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Clicks must be 0 or greater")]
    public int Clicks { get; set; }

    [Range(0, 100, ErrorMessage = "CTR must be between 0 and 100")]
    public decimal? CTR { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "CPM must be 0 or greater")]
    public decimal? CPM { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "CPC must be 0 or greater")]
    public decimal? CPC { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int? VideoId { get; set; }
    public int? CampaignId { get; set; }
}

public class UpdateAdRevenueDto
{
    [Required(ErrorMessage = "Amount is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Amount must be 0 or greater")]
    public decimal Amount { get; set; }

    [Range(0, int.MaxValue)]
    public int Impressions { get; set; }

    [Range(0, int.MaxValue)]
    public int Clicks { get; set; }

    [Range(0, 100)]
    public decimal? CTR { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? CPM { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? CPC { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
