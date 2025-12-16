using ProjectFinally.Models.Entities;

namespace ProjectFinally.Repositories.Interfaces;

public interface IVideoRepository : IGenericRepository<Video>
{
    Task<IEnumerable<Video>> GetVideosByChannelAsync(int channelId);
    Task<IEnumerable<Video>> GetVideosByCategoryAsync(int categoryId);
    Task<Video?> GetVideoWithAnalyticsAsync(int videoId);
    Task<IEnumerable<Video>> GetVideosByStatusAsync(string status);
    Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm);
    Task<IEnumerable<Video>> GetRecentVideosAsync(int count = 10);
    Task<IEnumerable<Video>> GetVideosByOwnerIdAsync(int ownerId);
}
