using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.YouTube;

public class VideoCategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateVideoCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Color { get; set; }
}

public class UpdateVideoCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Color { get; set; }

    public bool IsActive { get; set; }
}
