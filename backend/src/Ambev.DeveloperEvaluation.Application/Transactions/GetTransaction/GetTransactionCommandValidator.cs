using Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;

public class GetTransactionCommandValidator : AbstractValidator<GetTransactionCommand>
{
    public GetTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("ID cannot be empty");
    }
}