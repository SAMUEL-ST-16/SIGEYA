using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Validators.AdSense;

namespace ProjectFinally.Tests.Validators;

public class CreateAdSenseCampaignDtoValidatorTests
{
    private readonly CreateAdSenseCampaignDtoValidator _validator;

    public CreateAdSenseCampaignDtoValidatorTests()
    {
        _validator = new CreateAdSenseCampaignDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_CampaignName_Is_Empty()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto { CampaignName = "" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CampaignName);
    }

    [Fact]
    public void Should_Have_Error_When_CampaignName_Is_Too_Short()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto { CampaignName = "AB" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CampaignName)
            .WithErrorMessage("Campaign name must be at least 3 characters");
    }

    [Fact]
    public void Should_Have_Error_When_CampaignName_Exceeds_MaxLength()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto { CampaignName = new string('A', 201) };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CampaignName)
            .WithErrorMessage("Campaign name cannot exceed 200 characters");
    }

    [Fact]
    public void Should_Have_Error_When_Budget_Is_Zero()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto
        {
            CampaignName = "Valid Campaign",
            Budget = 0
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Budget)
            .WithErrorMessage("Budget must be greater than 0");
    }

    [Fact]
    public void Should_Have_Error_When_Budget_Is_Negative()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto
        {
            CampaignName = "Valid Campaign",
            Budget = -100
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Budget);
    }

    [Fact]
    public void Should_Have_Error_When_EndDate_Is_Before_StartDate()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto
        {
            CampaignName = "Valid Campaign",
            StartDate = DateTime.UtcNow.AddDays(10),
            EndDate = DateTime.UtcNow.AddDays(5),
            Budget = 1000,
            ChannelId = 1
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate)
            .WithErrorMessage("End date must be after start date");
    }

    [Fact]
    public void Should_Have_Error_When_ChannelId_Is_Zero()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto
        {
            CampaignName = "Valid Campaign",
            Budget = 1000,
            ChannelId = 0
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ChannelId)
            .WithErrorMessage("Channel ID must be greater than 0");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto
        {
            CampaignName = "Valid Campaign Name",
            Description = "A valid description",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            Budget = 5000,
            AdFormat = "Video",
            ChannelId = 1
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Not_Have_Error_When_Optional_Fields_Are_Null()
    {
        // Arrange
        var model = new CreateAdSenseCampaignDto
        {
            CampaignName = "Valid Campaign",
            StartDate = DateTime.UtcNow,
            Budget = 1000,
            ChannelId = 1,
            Description = null,
            EndDate = null,
            AdFormat = null
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
