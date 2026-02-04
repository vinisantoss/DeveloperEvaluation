using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransactionItem;

/// <summary>
/// Validator for CancelTransactionItemRequest
/// </summary>
public class CancelTransactionItemRequestValidator : AbstractValidator<CancelTransactionItemRequest>
{
    /// <summary>
    /// Initializes validation rules for CancelTransactionItemRequest
    /// </summary>
    public CancelTransactionItemRequestValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage("Transaction ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");
    }
}