using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Models.DTOs.YouTube;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Services.Implementations;

public class YouTubeChannelService : IYouTubeChannelService
{
    private readonly IGenericRepository<YouTubeChannel> _channelRepository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public YouTubeChannelService(IGenericRepository<YouTubeChannel> channelRepository, ApplicationDbContext context, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<YouTubeChannelDto>> GetAllChannelsAsync()
    {
        var channels = await _context.YouTubeChannels
            .Include(c => c.Videos)
            .ToListAsync();
        return _mapper.Map<IEnumerable<YouTubeChannelDto>>(channels);
    }

    public async Task<IEnumerable<YouTubeChannelDto>> GetChannelsByOwnerIdAsync(int ownerId)
    {
        var channels = await _context.YouTubeChannels
            .Include(c => c.Videos)
            .Where(c => c.OwnerId == ownerId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<YouTubeChannelDto>>(channels);
    }

    public async Task<YouTubeChannelDto?> GetChannelByIdAsync(int id)
    {
        var channel = await _channelRepository.GetByIdAsync(id);
        return channel == null ? null : _mapper.Map<YouTubeChannelDto>(channel);
    }

    public async Task<YouTubeChannelDto> CreateChannelAsync(CreateYouTubeChannelDto createChannelDto, int? ownerId = null)
    {
        var channel = _mapper.Map<YouTubeChannel>(createChannelDto);
        channel.CreatedAt = DateTime.UtcNow;
        channel.IsActive = true;
        channel.OwnerId = ownerId;

        await _channelRepository.AddAsync(channel);
        await _channelRepository.SaveChangesAsync();

        return _mapper.Map<YouTubeChannelDto>(channel);
    }

    public async Task<YouTubeChannelDto?> UpdateChannelAsync(int id, UpdateYouTubeChannelDto updateChannelDto)
    {
        var channel = await _channelRepository.GetByIdAsync(id);
        if (channel == null)
            return null;

        _mapper.Map(updateChannelDto, channel);
        channel.UpdatedAt = DateTime.UtcNow;

        _channelRepository.Update(channel);
        await _channelRepository.SaveChangesAsync();

        return _mapper.Map<YouTubeChannelDto>(channel);
    }

    public async Task<bool> DeleteChannelAsync(int id)
    {
        var channel = await _channelRepository.GetByIdAsync(id);
        if (channel == null)
            return false;

        _channelRepository.Delete(channel);
        return await _channelRepository.SaveChangesAsync();
    }
}
