using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;

namespace ProjectFinally.Repositories.Implementations;

public class VideoRepository : GenericRepository<Video>, IVideoRepository
{
    public VideoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Video>> GetVideosByChannelAsync(int channelId)
    {
        return await _dbSet
            .Where(v => v.ChannelId == channelId)
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .OrderByDescending(v => v.PublishedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Video>> GetVideosByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Where(v => v.CategoryId == categoryId)
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .OrderByDescending(v => v.PublishedAt)
            .ToListAsync();
    }

    public async Task<Video?> GetVideoWithAnalyticsAsync(int videoId)
    {
        return await _dbSet
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .Include(v => v.Analytics)
            .FirstOrDefaultAsync(v => v.VideoId == videoId);
    }

    public async Task<IEnumerable<Video>> GetVideosByStatusAsync(string status)
    {
        return await _dbSet
            .Where(v => v.Status == status)
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm)
    {
        return await _dbSet
            .Where(v => v.Title.Contains(searchTerm) ||
                       (v.Description != null && v.Description.Contains(searchTerm)) ||
                       (v.Tags != null && v.Tags.Contains(searchTerm)))
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .OrderByDescending(v => v.PublishedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Video>> GetRecentVideosAsync(int count = 10)
    {
        return await _dbSet
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .OrderByDescending(v => v.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Video>> GetVideosByOwnerIdAsync(int ownerId)
    {
        return await _dbSet
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .Include(v => v.Analytics)
            .Where(v => v.Channel != null && v.Channel.OwnerId == ownerId)
            .OrderByDescending(v => v.PublishedAt)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Video>> GetAllAsync()
    {
        return await _dbSet
            .Include(v => v.Channel)
            .Include(v => v.Category)
            .Include(v => v.Analytics)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }
}
