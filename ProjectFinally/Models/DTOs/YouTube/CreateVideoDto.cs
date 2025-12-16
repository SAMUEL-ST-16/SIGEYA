using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.YouTube;

public class CreateVideoDto
{
    [Required(ErrorMessage = "Titulo requerido")]
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
    public string Status { get; set; } = "Draft";

    [MaxLength(500)]
    public string? Tags { get; set; }

    [Required(ErrorMessage = "Channel ID is required")]
    public int ChannelId { get; set; }

    public int? CategoryId { get; set; }
}

public class UpdateVideoDto
{
    [Required(ErrorMessage = "Titulo requerido")]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    public int? DurationSeconds { get; set; }

    public DateTime? PublishedAt { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [MaxLength(500)]
    public string? Tags { get; set; }

    public int? CategoryId { get; set; }
}
