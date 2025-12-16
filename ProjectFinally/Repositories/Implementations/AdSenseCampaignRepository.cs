using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;

namespace ProjectFinally.Repositories.Implementations;

public class AdSenseCampaignRepository : GenericRepository<AdSenseCampaign>, IAdSenseCampaignRepository
{
    public AdSenseCampaignRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AdSenseCampaign>> GetCampaignsByChannelAsync(int channelId)
    {
        return await _dbSet
            .Where(c => c.ChannelId == channelId)
            .Include(c => c.Channel)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AdSenseCampaign>> GetCampaignsByOwnerIdAsync(int ownerId)
    {
        return await _dbSet
            .Include(c => c.Channel)
            .Where(c => c.Channel != null && c.Channel.OwnerId == ownerId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AdSenseCampaign>> GetActiveCampaignsAsync()
    {
        return await _dbSet
            .Where(c => c.IsActive)
            .Include(c => c.Channel)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<AdSenseCampaign>> GetCampaignsByStatusAsync(string status)
    {
        return await _dbSet
            .Where(c => c.Status == status)
            .Include(c => c.Channel)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<AdSenseCampaign?> GetCampaignWithRevenuesAsync(int campaignId)
    {
        return await _dbSet
            .Include(c => c.Channel)
            .Include(c => c.AdRevenues)
            .FirstOrDefaultAsync(c => c.CampaignId == campaignId);
    }
}
