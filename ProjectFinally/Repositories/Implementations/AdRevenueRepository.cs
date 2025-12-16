using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;

namespace ProjectFinally.Repositories.Implementations;

public class AdRevenueRepository : GenericRepository<AdRevenue>, IAdRevenueRepository
{
    public AdRevenueRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AdRevenue>> GetRevenuesByVideoAsync(int videoId)
    {
        return await _dbSet
            .Where(r => r.VideoId == videoId)
            .Include(r => r.Video)
            .Include(r => r.Campaign)
            .OrderByDescending(r => r.RevenueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<AdRevenue>> GetRevenuesByCampaignAsync(int campaignId)
    {
        return await _dbSet
            .Where(r => r.CampaignId == campaignId)
            .Include(r => r.Video)
            .Include(r => r.Campaign)
            .OrderByDescending(r => r.RevenueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<AdRevenue>> GetRevenuesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(r => r.RevenueDate >= startDate && r.RevenueDate <= endDate)
            .Include(r => r.Video)
            .Include(r => r.Campaign)
            .OrderByDescending(r => r.RevenueDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _dbSet.SumAsync(r => r.Amount);
    }

    public async Task<decimal> GetTotalRevenueByCampaignAsync(int campaignId)
    {
        return await _dbSet
            .Where(r => r.CampaignId == campaignId)
            .SumAsync(r => r.Amount);
    }
}
