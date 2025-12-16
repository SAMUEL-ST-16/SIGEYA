using FluentValidation;
using ProjectFinally.Models.DTOs.Tasks;

namespace ProjectFinally.Validators.Tasks;

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
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

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters")
            .Must(status => new[] { "Pending", "InProgress", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status must be: Pending, InProgress, Completed, or Cancelled");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Due date must be in the future")
            .When(x => x.DueDate.HasValue && x.Status != "Completed" && x.Status != "Cancelled");

        RuleFor(x => x.AssignedToEmployeeId)
            .GreaterThan(0).WithMessage("Assigned employee ID must be greater than 0")
            .When(x => x.AssignedToEmployeeId.HasValue);
    }
}
