using ProjectFinally.Models.DTOs.AdSense;

namespace ProjectFinally.Services.Interfaces;

public interface IAdSenseCampaignService
{
    Task<IEnumerable<AdSenseCampaignDto>> GetAllCampaignsAsync();
    Task<AdSenseCampaignDto?> GetCampaignByIdAsync(int id);
    Task<IEnumerable<AdSenseCampaignDto>> GetCampaignsByChannelAsync(int channelId);
    Task<IEnumerable<AdSenseCampaignDto>> GetCampaignsByOwnerIdAsync(int ownerId);
    Task<IEnumerable<AdSenseCampaignDto>> GetActiveCampaignsAsync();
    Task<IEnumerable<AdSenseCampaignDto>> GetCampaignsByStatusAsync(string status);
    Task<AdSenseCampaignDto> CreateCampaignAsync(CreateAdSenseCampaignDto createDto);
    Task<AdSenseCampaignDto?> UpdateCampaignAsync(int id, UpdateAdSenseCampaignDto updateDto);
    Task<bool> DeleteCampaignAsync(int id);
}
