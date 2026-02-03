using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Transactions.UpdateTransaction;

/// <summary>
/// Validator for UpdateTransactionCommand
/// </summary>
public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item operation is required");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateTransactionItemInfoValidator());
    }
}

/// <summary>
/// Validator for UpdateTransactionItemInfo
/// </summary>
public class UpdateTransactionItemInfoValidator : AbstractValidator<UpdateTransactionItemInfo>
{
    public UpdateTransactionItemInfoValidator()
    {
        RuleFor(x => x.Operation)
            .IsInEnum()
            .WithMessage("Valid operation is required");

        When(x => x.Operation == UpdateOperation.Update || x.Operation == UpdateOperation.Remove, () => {
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("Item ID is required for update/remove operations");
        });

        When(x => x.Operation == UpdateOperation.Add, () => {
            RuleFor(x => x.Product)
                .NotNull()
                .WithMessage("Product information is required for add operations")
                .SetValidator(new CommercialProductInfoValidator()!);

            RuleFor(x => x.ItemPrice)
                .NotNull()
                .WithMessage("Item price is required for add operations")
                .GreaterThan(0)
                .WithMessage("Item price must be greater than zero");
        });

        When(x => x.Operation == UpdateOperation.Add || x.Operation == UpdateOperation.Update, () => {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero")
                .LessThanOrEqualTo(20)
                .WithMessage("Cannot sell more than 20 identical items");
        });
    }
}

/// <summary>
/// Validator for CommercialProductInfo
/// </summary>
public class CommercialProductInfoValidator : AbstractValidator<CommercialProductInfo>
{
    public CommercialProductInfoValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Product external ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Product category is required");

        RuleFor(x => x.StandardPrice)
            .GreaterThan(0)
            .WithMessage("Product standard price must be greater than zero");
    }
}