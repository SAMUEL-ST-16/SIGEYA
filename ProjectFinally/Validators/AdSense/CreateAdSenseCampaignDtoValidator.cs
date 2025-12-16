using FluentValidation;
using ProjectFinally.Models.DTOs.AdSense;

namespace ProjectFinally.Validators.AdSense;

public class CreateAdSenseCampaignDtoValidator : AbstractValidator<CreateAdSenseCampaignDto>
{
    public CreateAdSenseCampaignDtoValidator()
    {
        RuleFor(x => x.CampaignName)
            .NotEmpty().WithMessage("Campaign name is required")
            .MaximumLength(200).WithMessage("Campaign name cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Campaign name must be at least 3 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1))
            .WithMessage("Start date cannot be more than 1 year in the future");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.Budget)
            .NotEmpty().WithMessage("Budget is required")
            .GreaterThan(0).WithMessage("Budget must be greater than 0")
            .LessThanOrEqualTo(1000000000).WithMessage("Budget cannot exceed 1,000,000,000");

        RuleFor(x => x.AdFormat)
            .MaximumLength(100).WithMessage("Ad format cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.AdFormat));

        RuleFor(x => x.ChannelId)
            .NotEmpty().WithMessage("Channel ID is required")
            .GreaterThan(0).WithMessage("Channel ID must be greater than 0");
    }
}
