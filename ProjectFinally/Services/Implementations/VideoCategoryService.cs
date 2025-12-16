using AutoMapper;
using ProjectFinally.Models.DTOs.YouTube;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Services.Implementations;

public class VideoCategoryService : IVideoCategoryService
{
    private readonly IGenericRepository<VideoCategory> _categoryRepository;
    private readonly IMapper _mapper;

    public VideoCategoryService(IGenericRepository<VideoCategory> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VideoCategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<VideoCategoryDto>>(categories);
    }

    public async Task<VideoCategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<VideoCategoryDto>(category);
    }

    public async Task<VideoCategoryDto> CreateCategoryAsync(CreateVideoCategoryDto createCategoryDto)
    {
        var category = _mapper.Map<VideoCategory>(createCategoryDto);
        category.CreatedAt = DateTime.UtcNow;
        category.IsActive = true;

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return _mapper.Map<VideoCategoryDto>(category);
    }

    public async Task<VideoCategoryDto?> UpdateCategoryAsync(int id, UpdateVideoCategoryDto updateCategoryDto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return null;

        _mapper.Map(updateCategoryDto, category);
        category.UpdatedAt = DateTime.UtcNow;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        return _mapper.Map<VideoCategoryDto>(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return false;

        _categoryRepository.Delete(category);
        return await _categoryRepository.SaveChangesAsync();
    }
}
