using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Transaction.Validation;

/// <summary>
/// Validator for TransactionItem entity with business rules
/// </summary>
public class TransactionItemValidator : AbstractValidator<TransactionItem>
{
    public TransactionItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product is required");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(item => item.ItemPrice)
            .GreaterThan(0)
            .WithMessage("Item price must be greater than zero");

        RuleFor(item => item.DiscountPercentage)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");

        // Business rule validation for discount
        RuleFor(item => item)
            .Must(ValidateDiscountRules)
            .WithMessage("Discount rules violation: purchases below 4 items cannot have discount, 4+ items get 10%, 10-20 items get 20%");
    }

    private bool ValidateDiscountRules(TransactionItem item)
    {
        return item.Quantity switch
        {
            < 4 => item.DiscountPercentage == 0,
            >= 4 and < 10 => item.DiscountPercentage == 10,
            >= 10 and <= 20 => item.DiscountPercentage == 20,
            _ => false
        };
    }
}