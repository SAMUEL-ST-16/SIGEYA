using ProjectFinally.Models.DTOs.YouTube;

namespace ProjectFinally.Services.Interfaces;

public interface IVideoService
{
    Task<IEnumerable<VideoDto>> GetAllVideosAsync();
    Task<VideoDto?> GetVideoByIdAsync(int id);
    Task<IEnumerable<VideoDto>> GetVideosByChannelAsync(int channelId);
    Task<IEnumerable<VideoDto>> GetVideosByCategoryAsync(int categoryId);
    Task<IEnumerable<VideoDto>> GetVideosByStatusAsync(string status);
    Task<IEnumerable<VideoDto>> SearchVideosAsync(string searchTerm);
    Task<IEnumerable<VideoDto>> GetVideosByOwnerIdAsync(int ownerId);
    Task<VideoDto> CreateVideoAsync(CreateVideoDto createVideoDto);
    Task<VideoDto?> UpdateVideoAsync(int id, UpdateVideoDto updateVideoDto);
    Task<bool> DeleteVideoAsync(int id);
}
