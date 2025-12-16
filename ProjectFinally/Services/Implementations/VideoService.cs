using AutoMapper;
using ProjectFinally.Models.DTOs.YouTube;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Services.Implementations;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly IMapper _mapper;

    public VideoService(IVideoRepository videoRepository, IMapper mapper)
    {
        _videoRepository = videoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VideoDto>> GetAllVideosAsync()
    {
        var videos = await _videoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<VideoDto>>(videos);
    }

    public async Task<VideoDto?> GetVideoByIdAsync(int id)
    {
        var video = await _videoRepository.GetVideoWithAnalyticsAsync(id);
        return video == null ? null : _mapper.Map<VideoDto>(video);
    }

    public async Task<IEnumerable<VideoDto>> GetVideosByChannelAsync(int channelId)
    {
        var videos = await _videoRepository.GetVideosByChannelAsync(channelId);
        return _mapper.Map<IEnumerable<VideoDto>>(videos);
    }

    public async Task<IEnumerable<VideoDto>> GetVideosByCategoryAsync(int categoryId)
    {
        var videos = await _videoRepository.GetVideosByCategoryAsync(categoryId);
        return _mapper.Map<IEnumerable<VideoDto>>(videos);
    }

    public async Task<IEnumerable<VideoDto>> GetVideosByStatusAsync(string status)
    {
        var videos = await _videoRepository.GetVideosByStatusAsync(status);
        return _mapper.Map<IEnumerable<VideoDto>>(videos);
    }

    public async Task<IEnumerable<VideoDto>> SearchVideosAsync(string searchTerm)
    {
        var videos = await _videoRepository.SearchVideosAsync(searchTerm);
        return _mapper.Map<IEnumerable<VideoDto>>(videos);
    }

    public async Task<IEnumerable<VideoDto>> GetVideosByOwnerIdAsync(int ownerId)
    {
        var videos = await _videoRepository.GetVideosByOwnerIdAsync(ownerId);
        return _mapper.Map<IEnumerable<VideoDto>>(videos);
    }

    public async Task<VideoDto> CreateVideoAsync(CreateVideoDto createVideoDto)
    {
        var video = _mapper.Map<Video>(createVideoDto);
        video.CreatedAt = DateTime.UtcNow;

        await _videoRepository.AddAsync(video);
        await _videoRepository.SaveChangesAsync();

        var createdVideo = await _videoRepository.GetVideoWithAnalyticsAsync(video.VideoId);
        return _mapper.Map<VideoDto>(createdVideo!);
    }

    public async Task<VideoDto?> UpdateVideoAsync(int id, UpdateVideoDto updateVideoDto)
    {
        var video = await _videoRepository.GetByIdAsync(id);
        if (video == null)
            return null;

        _mapper.Map(updateVideoDto, video);
        video.UpdatedAt = DateTime.UtcNow;

        _videoRepository.Update(video);
        await _videoRepository.SaveChangesAsync();

        var updatedVideo = await _videoRepository.GetVideoWithAnalyticsAsync(id);
        return _mapper.Map<VideoDto>(updatedVideo!);
    }

    public async Task<bool> DeleteVideoAsync(int id)
    {
        var video = await _videoRepository.GetByIdAsync(id);
        if (video == null)
            return false;

        _videoRepository.Delete(video);
        return await _videoRepository.SaveChangesAsync();
    }
}
