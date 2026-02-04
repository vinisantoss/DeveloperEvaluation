using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.ListCommercialTransactions;

/// <summary>
/// Validator for ListTransactionsRequest
/// </summary>
public class ListTransactionsRequestValidator : AbstractValidator<ListTransactionsRequest>
{
    /// <summary>
    /// Initializes validation rules for ListTransactionsRequest
    /// </summary>
    public ListTransactionsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Size cannot exceed 100");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Start date must be less than or equal to end date");
    }
}