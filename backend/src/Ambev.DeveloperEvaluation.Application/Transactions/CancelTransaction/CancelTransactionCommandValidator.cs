using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;

/// <summary>
/// Validator for CancelTransactionCommand
/// </summary>
public class CancelTransactionCommandValidator : AbstractValidator<CancelTransactionCommand>
{
    public CancelTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Transaction ID cannot be empty");
    }
}