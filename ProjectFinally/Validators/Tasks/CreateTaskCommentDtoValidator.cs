using FluentValidation;
using ProjectFinally.Models.DTOs.Tasks;

namespace ProjectFinally.Validators.Tasks;

public class CreateTaskCommentDtoValidator : AbstractValidator<CreateTaskCommentDto>
{
    public CreateTaskCommentDtoValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required")
            .MaximumLength(2000).WithMessage("Comment cannot exceed 2000 characters")
            .MinimumLength(1).WithMessage("Comment must be at least 1 character");

        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("Task ID is required")
            .GreaterThan(0).WithMessage("Task ID must be greater than 0");
    }
}
