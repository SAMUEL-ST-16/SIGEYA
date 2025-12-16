using ProjectFinally.Models.DTOs.YouTube;

namespace ProjectFinally.Services.Interfaces;

public interface IVideoCategoryService
{
    Task<IEnumerable<VideoCategoryDto>> GetAllCategoriesAsync();
    Task<VideoCategoryDto?> GetCategoryByIdAsync(int id);
    Task<VideoCategoryDto> CreateCategoryAsync(CreateVideoCategoryDto createCategoryDto);
    Task<VideoCategoryDto?> UpdateCategoryAsync(int id, UpdateVideoCategoryDto updateCategoryDto);
    Task<bool> DeleteCategoryAsync(int id);
}
