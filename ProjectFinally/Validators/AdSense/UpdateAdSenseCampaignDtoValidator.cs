using FluentValidation;
using ProjectFinally.Models.DTOs.AdSense;

namespace ProjectFinally.Validators.AdSense;

public class UpdateAdSenseCampaignDtoValidator : AbstractValidator<UpdateAdSenseCampaignDto>
{
    public UpdateAdSenseCampaignDtoValidator()
    {
        RuleFor(x => x.CampaignName)
            .NotEmpty().WithMessage("Campaign name is required")
            .MaximumLength(200).WithMessage("Campaign name cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Campaign name must be at least 3 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("End date must be in the future")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.Budget)
            .GreaterThan(0).WithMessage("Budget must be greater than 0")
            .LessThanOrEqualTo(1000000000).WithMessage("Budget cannot exceed 1,000,000,000");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters")
            .Must(status => new[] { "Active", "Paused", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status must be: Active, Paused, Completed, or Cancelled");

        RuleFor(x => x.AdFormat)
            .MaximumLength(100).WithMessage("Ad format cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.AdFormat));
    }
}
