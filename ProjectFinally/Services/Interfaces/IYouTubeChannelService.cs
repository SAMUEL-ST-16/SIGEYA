using ProjectFinally.Models.DTOs.YouTube;

namespace ProjectFinally.Services.Interfaces;

public interface IYouTubeChannelService
{
    Task<IEnumerable<YouTubeChannelDto>> GetAllChannelsAsync();
    Task<IEnumerable<YouTubeChannelDto>> GetChannelsByOwnerIdAsync(int ownerId);
    Task<YouTubeChannelDto?> GetChannelByIdAsync(int id);
    Task<YouTubeChannelDto> CreateChannelAsync(CreateYouTubeChannelDto createChannelDto, int? ownerId = null);
    Task<YouTubeChannelDto?> UpdateChannelAsync(int id, UpdateYouTubeChannelDto updateChannelDto);
    Task<bool> DeleteChannelAsync(int id);
}
