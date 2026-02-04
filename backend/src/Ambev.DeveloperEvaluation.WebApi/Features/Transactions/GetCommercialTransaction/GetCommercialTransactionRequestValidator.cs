using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.GetCommercialTransaction;

/// <summary>
/// Validator for GetTransactionRequest
/// </summary>
public class GetTransactionRequestValidator : AbstractValidator<GetCommercialTransactionRequest>
{
    /// <summary>
    /// Initializes validation rules for GetTransactionRequest
    /// </summary>
    public GetTransactionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required");
    }
}