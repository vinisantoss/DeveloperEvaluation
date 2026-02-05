using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for OperationalUnit entity
/// </summary>
public class OperationalUnitValidator : AbstractValidator<OperationalUnit>
{
    /// <summary>
    /// Initializes validation rules for OperationalUnit
    /// </summary>
    public OperationalUnitValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("External ID is required")
            .MaximumLength(50)
            .WithMessage("External ID must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required")
            .MaximumLength(200)
            .WithMessage("Location must not exceed 200 characters");
    }
}