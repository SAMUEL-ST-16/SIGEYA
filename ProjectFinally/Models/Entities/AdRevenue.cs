using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinally.Models.Entities;

public class AdRevenue
{
    [Key]
    public int RevenueId { get; set; }

    public DateTime RevenueDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public int Impressions { get; set; } = 0;
    public int Clicks { get; set; } = 0;

    [Column(TypeName = "decimal(5,2)")]
    public decimal? CTR { get; set; } // Click-Through Rate

    [Column(TypeName = "decimal(18,4)")]
    public decimal? CPM { get; set; } // Cost Per Mille (1000 impressions)

    [Column(TypeName = "decimal(18,4)")]
    public decimal? CPC { get; set; } // Cost Per Click

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int? VideoId { get; set; }
    public int? CampaignId { get; set; }

    // Navigation Properties
    public Video? Video { get; set; }
    public AdSenseCampaign? Campaign { get; set; }
}
