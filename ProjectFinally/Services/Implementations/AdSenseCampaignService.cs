using AutoMapper;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Services.Implementations;

public class AdSenseCampaignService : IAdSenseCampaignService
{
    private readonly IAdSenseCampaignRepository _campaignRepository;
    private readonly IMapper _mapper;

    public AdSenseCampaignService(IAdSenseCampaignRepository campaignRepository, IMapper mapper)
    {
        _campaignRepository = campaignRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AdSenseCampaignDto>> GetAllCampaignsAsync()
    {
        var campaigns = await _campaignRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AdSenseCampaignDto>>(campaigns);
    }

    public async Task<AdSenseCampaignDto?> GetCampaignByIdAsync(int id)
    {
        var campaign = await _campaignRepository.GetCampaignWithRevenuesAsync(id);
        return campaign == null ? null : _mapper.Map<AdSenseCampaignDto>(campaign);
    }

    public async Task<IEnumerable<AdSenseCampaignDto>> GetCampaignsByChannelAsync(int channelId)
    {
        var campaigns = await _campaignRepository.GetCampaignsByChannelAsync(channelId);
        return _mapper.Map<IEnumerable<AdSenseCampaignDto>>(campaigns);
    }

    public async Task<IEnumerable<AdSenseCampaignDto>> GetCampaignsByOwnerIdAsync(int ownerId)
    {
        var campaigns = await _campaignRepository.GetCampaignsByOwnerIdAsync(ownerId);
        return _mapper.Map<IEnumerable<AdSenseCampaignDto>>(campaigns);
    }

    public async Task<IEnumerable<AdSenseCampaignDto>> GetActiveCampaignsAsync()
    {
        var campaigns = await _campaignRepository.GetActiveCampaignsAsync();
        return _mapper.Map<IEnumerable<AdSenseCampaignDto>>(campaigns);
    }

    public async Task<IEnumerable<AdSenseCampaignDto>> GetCampaignsByStatusAsync(string status)
    {
        var campaigns = await _campaignRepository.GetCampaignsByStatusAsync(status);
        return _mapper.Map<IEnumerable<AdSenseCampaignDto>>(campaigns);
    }

    public async Task<AdSenseCampaignDto> CreateCampaignAsync(CreateAdSenseCampaignDto createDto)
    {
        var campaign = _mapper.Map<AdSenseCampaign>(createDto);
        campaign.CreatedAt = DateTime.UtcNow;
        campaign.Status = "Active";
        campaign.IsActive = true;
        campaign.CurrentSpent = 0;

        await _campaignRepository.AddAsync(campaign);
        await _campaignRepository.SaveChangesAsync();

        var createdCampaign = await _campaignRepository.GetCampaignWithRevenuesAsync(campaign.CampaignId);
        return _mapper.Map<AdSenseCampaignDto>(createdCampaign!);
    }

    public async Task<AdSenseCampaignDto?> UpdateCampaignAsync(int id, UpdateAdSenseCampaignDto updateDto)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id);
        if (campaign == null)
            return null;

        _mapper.Map(updateDto, campaign);
        campaign.UpdatedAt = DateTime.UtcNow;

        _campaignRepository.Update(campaign);
        await _campaignRepository.SaveChangesAsync();

        var updatedCampaign = await _campaignRepository.GetCampaignWithRevenuesAsync(id);
        return _mapper.Map<AdSenseCampaignDto>(updatedCampaign!);
    }

    public async Task<bool> DeleteCampaignAsync(int id)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id);
        if (campaign == null)
            return false;

        _campaignRepository.Delete(campaign);
        return await _campaignRepository.SaveChangesAsync();
    }
}
