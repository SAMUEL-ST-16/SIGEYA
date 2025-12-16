using ProjectFinally.Models.Entities;

namespace ProjectFinally.Repositories.Interfaces;

public interface IAdRevenueRepository : IGenericRepository<AdRevenue>
{
    Task<IEnumerable<AdRevenue>> GetRevenuesByVideoAsync(int videoId);
    Task<IEnumerable<AdRevenue>> GetRevenuesByCampaignAsync(int campaignId);
    Task<IEnumerable<AdRevenue>> GetRevenuesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetTotalRevenueByCampaignAsync(int campaignId);
}
