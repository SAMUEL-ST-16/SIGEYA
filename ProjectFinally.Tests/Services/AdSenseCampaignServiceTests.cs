using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;
using ProjectFinally.Services.Implementations;

namespace ProjectFinally.Tests.Services;

public class AdSenseCampaignServiceTests
{
    private readonly Mock<IAdSenseCampaignRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AdSenseCampaignService _service;

    public AdSenseCampaignServiceTests()
    {
        _mockRepository = new Mock<IAdSenseCampaignRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new AdSenseCampaignService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllCampaignsAsync_ShouldReturnAllCampaigns()
    {
        // Arrange
        var campaigns = new List<AdSenseCampaign>
        {
            new AdSenseCampaign { CampaignId = 1, CampaignName = "Campaign 1" },
            new AdSenseCampaign { CampaignId = 2, CampaignName = "Campaign 2" }
        };

        var campaignDtos = new List<AdSenseCampaignDto>
        {
            new AdSenseCampaignDto { CampaignId = 1, CampaignName = "Campaign 1" },
            new AdSenseCampaignDto { CampaignId = 2, CampaignName = "Campaign 2" }
        };

        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(campaigns);

        _mockMapper.Setup(m => m.Map<IEnumerable<AdSenseCampaignDto>>(campaigns))
            .Returns(campaignDtos);

        // Act
        var result = await _service.GetAllCampaignsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(campaignDtos);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetCampaignByIdAsync_WithValidId_ShouldReturnCampaign()
    {
        // Arrange
        var campaignId = 1;
        var campaign = new AdSenseCampaign
        {
            CampaignId = campaignId,
            CampaignName = "Test Campaign",
            Budget = 5000m
        };

        var campaignDto = new AdSenseCampaignDto
        {
            CampaignId = campaignId,
            CampaignName = "Test Campaign",
            Budget = 5000m
        };

        _mockRepository.Setup(r => r.GetCampaignWithRevenuesAsync(campaignId))
            .ReturnsAsync(campaign);

        _mockMapper.Setup(m => m.Map<AdSenseCampaignDto>(campaign))
            .Returns(campaignDto);

        // Act
        var result = await _service.GetCampaignByIdAsync(campaignId);

        // Assert
        result.Should().NotBeNull();
        result.CampaignId.Should().Be(campaignId);
        result.CampaignName.Should().Be("Test Campaign");
        result.Budget.Should().Be(5000m);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetCampaignByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var campaignId = 999;
        _mockRepository.Setup(r => r.GetCampaignWithRevenuesAsync(campaignId))
            .ReturnsAsync((AdSenseCampaign?)null);

        // Act
        var result = await _service.GetCampaignByIdAsync(campaignId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateCampaignAsync_WithValidData_ShouldCreateCampaign()
    {
        // Arrange
        var createDto = new CreateAdSenseCampaignDto
        {
            CampaignName = "New Campaign",
            Budget = 10000m,
            ChannelId = 1
        };

        var campaign = new AdSenseCampaign
        {
            CampaignId = 0,
            CampaignName = "New Campaign",
            Budget = 10000m,
            ChannelId = 1
        };

        var createdCampaign = new AdSenseCampaign
        {
            CampaignId = 1,
            CampaignName = "New Campaign",
            Budget = 10000m,
            ChannelId = 1,
            Status = "Active",
            IsActive = true
        };

        var campaignDto = new AdSenseCampaignDto
        {
            CampaignId = 1,
            CampaignName = "New Campaign",
            Budget = 10000m
        };

        _mockMapper.Setup(m => m.Map<AdSenseCampaign>(createDto))
            .Returns(campaign);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<AdSenseCampaign>()))
            .ReturnsAsync(campaign);

        _mockRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(true);

        _mockRepository.Setup(r => r.GetCampaignWithRevenuesAsync(It.IsAny<int>()))
            .ReturnsAsync(createdCampaign);

        _mockMapper.Setup(m => m.Map<AdSenseCampaignDto>(createdCampaign))
            .Returns(campaignDto);

        // Act
        var result = await _service.CreateCampaignAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.CampaignName.Should().Be("New Campaign");
        result.Budget.Should().Be(10000m);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<AdSenseCampaign>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetActiveCampaignsAsync_ShouldReturnOnlyActiveCampaigns()
    {
        // Arrange
        var activeCampaigns = new List<AdSenseCampaign>
        {
            new AdSenseCampaign { CampaignId = 1, CampaignName = "Active 1", IsActive = true },
            new AdSenseCampaign { CampaignId = 2, CampaignName = "Active 2", IsActive = true }
        };

        var campaignDtos = new List<AdSenseCampaignDto>
        {
            new AdSenseCampaignDto { CampaignId = 1, CampaignName = "Active 1" },
            new AdSenseCampaignDto { CampaignId = 2, CampaignName = "Active 2" }
        };

        _mockRepository.Setup(r => r.GetActiveCampaignsAsync())
            .ReturnsAsync(activeCampaigns);

        _mockMapper.Setup(m => m.Map<IEnumerable<AdSenseCampaignDto>>(activeCampaigns))
            .Returns(campaignDtos);

        // Act
        var result = await _service.GetActiveCampaignsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(c => c.CampaignName.Contains("Active"));
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteCampaignAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var campaignId = 1;
        var campaign = new AdSenseCampaign { CampaignId = campaignId };

        _mockRepository.Setup(r => r.GetByIdAsync(campaignId))
            .ReturnsAsync(campaign);

        _mockRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteCampaignAsync(campaignId);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.Delete(campaign), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteCampaignAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var campaignId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(campaignId))
            .ReturnsAsync((AdSenseCampaign?)null);

        // Act
        var result = await _service.DeleteCampaignAsync(campaignId);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.Delete(It.IsAny<AdSenseCampaign>()), Times.Never);
    }
}
