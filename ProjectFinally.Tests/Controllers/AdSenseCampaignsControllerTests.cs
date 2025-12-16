using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectFinally.Controllers;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Tests.Controllers;

public class AdSenseCampaignsControllerTests
{
    private readonly Mock<IAdSenseCampaignService> _mockService;
    private readonly Mock<ILogger<AdSenseCampaignsController>> _mockLogger;
    private readonly AdSenseCampaignsController _controller;

    public AdSenseCampaignsControllerTests()
    {
        _mockService = new Mock<IAdSenseCampaignService>();
        _mockLogger = new Mock<ILogger<AdSenseCampaignsController>>();
        _controller = new AdSenseCampaignsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllCampaigns_ShouldReturnOkWithCampaigns()
    {
        // Arrange
        var campaigns = new List<AdSenseCampaignDto>
        {
            new AdSenseCampaignDto { CampaignId = 1, CampaignName = "Campaign 1" },
            new AdSenseCampaignDto { CampaignId = 2, CampaignName = "Campaign 2" }
        };

        _mockService.Setup(s => s.GetAllCampaignsAsync())
            .ReturnsAsync(campaigns);

        // Act
        var result = await _controller.GetAllCampaigns();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCampaigns = okResult.Value.Should().BeAssignableTo<IEnumerable<AdSenseCampaignDto>>().Subject;
        returnedCampaigns.Should().HaveCount(2);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetCampaign_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var campaignId = 1;
        var campaign = new AdSenseCampaignDto
        {
            CampaignId = campaignId,
            CampaignName = "Test Campaign"
        };

        _mockService.Setup(s => s.GetCampaignByIdAsync(campaignId))
            .ReturnsAsync(campaign);

        // Act
        var result = await _controller.GetCampaign(campaignId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCampaign = okResult.Value.Should().BeAssignableTo<AdSenseCampaignDto>().Subject;
        returnedCampaign.CampaignId.Should().Be(campaignId);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetCampaign_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var campaignId = 999;
        _mockService.Setup(s => s.GetCampaignByIdAsync(campaignId))
            .ReturnsAsync((AdSenseCampaignDto?)null);

        // Act
        var result = await _controller.GetCampaign(campaignId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateCampaign_WithValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateAdSenseCampaignDto
        {
            CampaignName = "New Campaign",
            Budget = 10000m,
            ChannelId = 1
        };

        var createdCampaign = new AdSenseCampaignDto
        {
            CampaignId = 1,
            CampaignName = "New Campaign",
            Budget = 10000m
        };

        _mockService.Setup(s => s.CreateCampaignAsync(createDto))
            .ReturnsAsync(createdCampaign);

        // Act
        var result = await _controller.CreateCampaign(createDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(AdSenseCampaignsController.GetCampaign));
        var returnedCampaign = createdResult.Value.Should().BeAssignableTo<AdSenseCampaignDto>().Subject;
        returnedCampaign.CampaignName.Should().Be("New Campaign");
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateCampaign_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var campaignId = 1;
        var updateDto = new UpdateAdSenseCampaignDto
        {
            CampaignName = "Updated Campaign",
            Budget = 15000m
        };

        var updatedCampaign = new AdSenseCampaignDto
        {
            CampaignId = campaignId,
            CampaignName = "Updated Campaign",
            Budget = 15000m
        };

        _mockService.Setup(s => s.UpdateCampaignAsync(campaignId, updateDto))
            .ReturnsAsync(updatedCampaign);

        // Act
        var result = await _controller.UpdateCampaign(campaignId, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCampaign = okResult.Value.Should().BeAssignableTo<AdSenseCampaignDto>().Subject;
        returnedCampaign.CampaignName.Should().Be("Updated Campaign");
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateCampaign_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var campaignId = 999;
        var updateDto = new UpdateAdSenseCampaignDto
        {
            CampaignName = "Updated Campaign",
            Budget = 15000m
        };

        _mockService.Setup(s => s.UpdateCampaignAsync(campaignId, updateDto))
            .ReturnsAsync((AdSenseCampaignDto?)null);

        // Act
        var result = await _controller.UpdateCampaign(campaignId, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteCampaign_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var campaignId = 1;
        _mockService.Setup(s => s.DeleteCampaignAsync(campaignId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteCampaign(campaignId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteCampaign_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var campaignId = 999;
        _mockService.Setup(s => s.DeleteCampaignAsync(campaignId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteCampaign(campaignId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetActiveCampaigns_ShouldReturnOkWithActiveCampaigns()
    {
        // Arrange
        var activeCampaigns = new List<AdSenseCampaignDto>
        {
            new AdSenseCampaignDto { CampaignId = 1, CampaignName = "Active 1", IsActive = true },
            new AdSenseCampaignDto { CampaignId = 2, CampaignName = "Active 2", IsActive = true }
        };

        _mockService.Setup(s => s.GetActiveCampaignsAsync())
            .ReturnsAsync(activeCampaigns);

        // Act
        var result = await _controller.GetActiveCampaigns();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCampaigns = okResult.Value.Should().BeAssignableTo<IEnumerable<AdSenseCampaignDto>>().Subject;
        returnedCampaigns.Should().HaveCount(2);
        returnedCampaigns.Should().OnlyContain(c => c.IsActive);
    }
}
