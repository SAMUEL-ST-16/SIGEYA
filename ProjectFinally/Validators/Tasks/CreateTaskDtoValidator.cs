using FluentValidation;
using ProjectFinally.Models.DTOs.Tasks;

namespace ProjectFinally.Validators.Tasks;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .MaximumLength(50).WithMessage("Priority cannot exceed 50 characters")
            .Must(priority => new[] { "Low", "Medium", "High", "Urgent" }.Contains(priority))
            .WithMessage("Priority must be: Low, Medium, High, or Urgent");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Due date must be in the future")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.AssignedToEmployeeId)
            .GreaterThan(0).WithMessage("Assigned employee ID must be greater than 0")
            .When(x => x.AssignedToEmployeeId.HasValue);
    }
}
