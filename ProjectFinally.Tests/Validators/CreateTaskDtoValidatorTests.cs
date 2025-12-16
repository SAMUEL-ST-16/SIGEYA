using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectFinally.Models.DTOs.Tasks;
using ProjectFinally.Validators.Tasks;

namespace ProjectFinally.Tests.Validators;

public class CreateTaskDtoValidatorTests
{
    private readonly CreateTaskDtoValidator _validator;

    public CreateTaskDtoValidatorTests()
    {
        _validator = new CreateTaskDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange
        var model = new CreateTaskDto { Title = "" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Short()
    {
        // Arrange
        var model = new CreateTaskDto { Title = "AB" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must be at least 3 characters");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_MaxLength()
    {
        // Arrange
        var model = new CreateTaskDto { Title = new string('A', 201) };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title cannot exceed 200 characters");
    }

    [Fact]
    public void Should_Have_Error_When_Priority_Is_Invalid()
    {
        // Arrange
        var model = new CreateTaskDto
        {
            Title = "Valid Title",
            Priority = "InvalidPriority"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Priority)
            .WithErrorMessage("Priority must be: Low, Medium, High, or Urgent");
    }

    [Theory]
    [InlineData("Low")]
    [InlineData("Medium")]
    [InlineData("High")]
    [InlineData("Urgent")]
    public void Should_Not_Have_Error_When_Priority_Is_Valid(string priority)
    {
        // Arrange
        var model = new CreateTaskDto
        {
            Title = "Valid Title",
            Priority = priority
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Fact]
    public void Should_Have_Error_When_DueDate_Is_In_The_Past()
    {
        // Arrange
        var model = new CreateTaskDto
        {
            Title = "Valid Title",
            Priority = "Medium",
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DueDate)
            .WithErrorMessage("Due date must be in the future");
    }

    [Fact]
    public void Should_Have_Error_When_AssignedToEmployeeId_Is_Zero()
    {
        // Arrange
        var model = new CreateTaskDto
        {
            Title = "Valid Title",
            Priority = "Medium",
            AssignedToEmployeeId = 0
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AssignedToEmployeeId)
            .WithErrorMessage("Assigned employee ID must be greater than 0");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        // Arrange
        var model = new CreateTaskDto
        {
            Title = "Valid Task Title",
            Description = "A valid description",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(7),
            AssignedToEmployeeId = 1
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
        var model = new CreateTaskDto
        {
            Title = "Valid Title",
            Priority = "Medium",
            Description = null,
            DueDate = null,
            AssignedToEmployeeId = null
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
