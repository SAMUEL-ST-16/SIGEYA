using ProjectFinally.Models.Entities;

namespace ProjectFinally.Repositories.Interfaces;

public interface IAdSenseCampaignRepository : IGenericRepository<AdSenseCampaign>
{
    Task<IEnumerable<AdSenseCampaign>> GetCampaignsByChannelAsync(int channelId);
    Task<IEnumerable<AdSenseCampaign>> GetCampaignsByOwnerIdAsync(int ownerId);
    Task<IEnumerable<AdSenseCampaign>> GetActiveCampaignsAsync();
    Task<IEnumerable<AdSenseCampaign>> GetCampaignsByStatusAsync(string status);
    Task<AdSenseCampaign?> GetCampaignWithRevenuesAsync(int campaignId);
}
