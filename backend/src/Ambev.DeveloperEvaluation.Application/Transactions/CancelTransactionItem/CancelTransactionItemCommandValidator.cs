using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;

public class CancelTransactionItemCommandValidator : AbstractValidator<CancelTransactionItemCommand>
{
    public CancelTransactionItemCommandValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage("Transaction ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Transaction ID cannot be empty");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Item ID cannot be empty");
    }
}