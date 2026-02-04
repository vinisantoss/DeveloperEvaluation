using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransaction;

/// <summary>
/// Validator for CancelTransactionRequest
/// </summary>
public class CancelTransactionRequestValidator : AbstractValidator<CancelTransactionRequest>
{
    /// <summary>
    /// Initializes validation rules for CancelTransactionRequest
    /// </summary>
    public CancelTransactionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required");
    }
}
