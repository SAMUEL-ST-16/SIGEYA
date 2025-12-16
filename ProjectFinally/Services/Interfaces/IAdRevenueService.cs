using ProjectFinally.Models.DTOs.AdSense;

namespace ProjectFinally.Services.Interfaces;

public interface IAdRevenueService
{
    Task<IEnumerable<AdRevenueDto>> GetAllRevenuesAsync();
    Task<AdRevenueDto?> GetRevenueByIdAsync(int id);
    Task<IEnumerable<AdRevenueDto>> GetRevenuesByVideoAsync(int videoId);
    Task<IEnumerable<AdRevenueDto>> GetRevenuesByCampaignAsync(int campaignId);
    Task<IEnumerable<AdRevenueDto>> GetRevenuesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetTotalRevenueByCampaignAsync(int campaignId);
    Task<AdRevenueDto> CreateRevenueAsync(CreateAdRevenueDto createDto);
    Task<AdRevenueDto?> UpdateRevenueAsync(int id, UpdateAdRevenueDto updateDto);
    Task<bool> DeleteRevenueAsync(int id);
}
