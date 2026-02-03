using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Transaction.Validation;

/// <summary>
/// Validator for CommercialTransaction entity with business rules
/// </summary>
public class CommercialTransactionValidator : AbstractValidator<CommercialTransaction>
{
    public CommercialTransactionValidator()
    {
        RuleFor(transaction => transaction.TransactionCode)
            .NotEmpty()
            .WithMessage("Transaction code is required")
            .MaximumLength(50)
            .WithMessage("Transaction code cannot exceed 50 characters");

        RuleFor(transaction => transaction.BusinessPartnerId)
            .NotEmpty()
            .WithMessage("Business partner is required");

        RuleFor(transaction => transaction.OperationalUnitId)
            .NotEmpty()
            .WithMessage("Operational unit is required");

        RuleFor(transaction => transaction.TransactionDate)
            .NotEmpty()
            .WithMessage("Transaction date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Transaction date cannot be in the future");

        RuleFor(transaction => transaction.Items)
            .NotEmpty()
            .WithMessage("Transaction must have at least one item");

        RuleForEach(transaction => transaction.Items)
            .SetValidator(new TransactionItemValidator());
    }
}