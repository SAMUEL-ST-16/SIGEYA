using FluentValidation;
using ProjectFinally.Models.DTOs.AdSense;

namespace ProjectFinally.Validators.AdSense;

public class UpdateAdRevenueDtoValidator : AbstractValidator<UpdateAdRevenueDto>
{
    public UpdateAdRevenueDtoValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be 0 or greater")
            .LessThanOrEqualTo(1000000000).WithMessage("Amount cannot exceed 1,000,000,000");

        RuleFor(x => x.Impressions)
            .GreaterThanOrEqualTo(0).WithMessage("Impressions must be 0 or greater");

        RuleFor(x => x.Clicks)
            .GreaterThanOrEqualTo(0).WithMessage("Clicks must be 0 or greater")
            .LessThanOrEqualTo(x => x.Impressions)
            .WithMessage("Clicks cannot exceed impressions")
            .When(x => x.Impressions > 0);

        RuleFor(x => x.CTR)
            .InclusiveBetween(0, 100).WithMessage("CTR must be between 0 and 100")
            .When(x => x.CTR.HasValue);

        RuleFor(x => x.CPM)
            .GreaterThanOrEqualTo(0).WithMessage("CPM must be 0 or greater")
            .LessThanOrEqualTo(10000).WithMessage("CPM cannot exceed 10,000")
            .When(x => x.CPM.HasValue);

        RuleFor(x => x.CPC)
            .GreaterThanOrEqualTo(0).WithMessage("CPC must be 0 or greater")
            .LessThanOrEqualTo(1000).WithMessage("CPC cannot exceed 1,000")
            .When(x => x.CPC.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
