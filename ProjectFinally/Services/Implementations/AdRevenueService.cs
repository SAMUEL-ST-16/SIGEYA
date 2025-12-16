using AutoMapper;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Services.Implementations;

public class AdRevenueService : IAdRevenueService
{
    private readonly IAdRevenueRepository _revenueRepository;
    private readonly IAdSenseCampaignRepository _campaignRepository;
    private readonly IMapper _mapper;

    public AdRevenueService(
        IAdRevenueRepository revenueRepository,
        IAdSenseCampaignRepository campaignRepository,
        IMapper mapper)
    {
        _revenueRepository = revenueRepository;
        _campaignRepository = campaignRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AdRevenueDto>> GetAllRevenuesAsync()
    {
        var revenues = await _revenueRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AdRevenueDto>>(revenues);
    }

    public async Task<AdRevenueDto?> GetRevenueByIdAsync(int id)
    {
        var revenue = await _revenueRepository.GetByIdAsync(id);
        return revenue == null ? null : _mapper.Map<AdRevenueDto>(revenue);
    }

    public async Task<IEnumerable<AdRevenueDto>> GetRevenuesByVideoAsync(int videoId)
    {
        var revenues = await _revenueRepository.GetRevenuesByVideoAsync(videoId);
        return _mapper.Map<IEnumerable<AdRevenueDto>>(revenues);
    }

    public async Task<IEnumerable<AdRevenueDto>> GetRevenuesByCampaignAsync(int campaignId)
    {
        var revenues = await _revenueRepository.GetRevenuesByCampaignAsync(campaignId);
        return _mapper.Map<IEnumerable<AdRevenueDto>>(revenues);
    }

    public async Task<IEnumerable<AdRevenueDto>> GetRevenuesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var revenues = await _revenueRepository.GetRevenuesByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<AdRevenueDto>>(revenues);
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _revenueRepository.GetTotalRevenueAsync();
    }

    public async Task<decimal> GetTotalRevenueByCampaignAsync(int campaignId)
    {
        return await _revenueRepository.GetTotalRevenueByCampaignAsync(campaignId);
    }

    public async Task<AdRevenueDto> CreateRevenueAsync(CreateAdRevenueDto createDto)
    {
        var revenue = _mapper.Map<AdRevenue>(createDto);
        revenue.CreatedAt = DateTime.UtcNow;

        await _revenueRepository.AddAsync(revenue);
        await _revenueRepository.SaveChangesAsync();

        // Update campaign CurrentSpent if revenue is linked to a campaign
        if (revenue.CampaignId.HasValue)
        {
            var campaign = await _campaignRepository.GetByIdAsync(revenue.CampaignId.Value);
            if (campaign != null)
            {
                campaign.CurrentSpent += revenue.Amount;
                campaign.UpdatedAt = DateTime.UtcNow;
                _campaignRepository.Update(campaign);
                await _campaignRepository.SaveChangesAsync();
            }
        }

        var createdRevenue = await _revenueRepository.GetByIdAsync(revenue.RevenueId);
        return _mapper.Map<AdRevenueDto>(createdRevenue!);
    }

    public async Task<AdRevenueDto?> UpdateRevenueAsync(int id, UpdateAdRevenueDto updateDto)
    {
        var revenue = await _revenueRepository.GetByIdAsync(id);
        if (revenue == null)
            return null;

        var oldAmount = revenue.Amount;
        _mapper.Map(updateDto, revenue);
        revenue.UpdatedAt = DateTime.UtcNow;

        _revenueRepository.Update(revenue);
        await _revenueRepository.SaveChangesAsync();

        // Update campaign CurrentSpent if amount changed
        if (revenue.CampaignId.HasValue && oldAmount != revenue.Amount)
        {
            var campaign = await _campaignRepository.GetByIdAsync(revenue.CampaignId.Value);
            if (campaign != null)
            {
                campaign.CurrentSpent = campaign.CurrentSpent - oldAmount + revenue.Amount;
                campaign.UpdatedAt = DateTime.UtcNow;
                _campaignRepository.Update(campaign);
                await _campaignRepository.SaveChangesAsync();
            }
        }

        var updatedRevenue = await _revenueRepository.GetByIdAsync(id);
        return _mapper.Map<AdRevenueDto>(updatedRevenue!);
    }

    public async Task<bool> DeleteRevenueAsync(int id)
    {
        var revenue = await _revenueRepository.GetByIdAsync(id);
        if (revenue == null)
            return false;

        // Update campaign CurrentSpent if revenue is linked to a campaign
        if (revenue.CampaignId.HasValue)
        {
            var campaign = await _campaignRepository.GetByIdAsync(revenue.CampaignId.Value);
            if (campaign != null)
            {
                campaign.CurrentSpent -= revenue.Amount;
                campaign.UpdatedAt = DateTime.UtcNow;
                _campaignRepository.Update(campaign);
                await _campaignRepository.SaveChangesAsync();
            }
        }

        _revenueRepository.Delete(revenue);
        return await _revenueRepository.SaveChangesAsync();
    }
}
