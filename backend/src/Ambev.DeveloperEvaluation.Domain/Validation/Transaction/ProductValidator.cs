using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Product entity
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    /// <summary>
    /// Initializes validation rules for Product
    /// </summary>
    public ProductValidator()
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

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters");

        RuleFor(x => x.StandardPrice)
            .GreaterThan(0)
            .WithMessage("Standard price must be greater than zero")
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Standard price must not exceed 999,999.99");
    }
}