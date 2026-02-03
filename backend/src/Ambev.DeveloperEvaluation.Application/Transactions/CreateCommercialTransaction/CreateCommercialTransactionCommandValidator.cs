using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;

/// <summary>
/// Validator for CreateCommercialTransactionCommand
/// </summary>
public class CreateCommercialTransactionCommandValidator : AbstractValidator<CreateCommercialTransactionCommand>
{
    public CreateCommercialTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionCode)
            .NotEmpty()
            .WithMessage("Transaction code is required")
            .MaximumLength(50)
            .WithMessage("Transaction code cannot exceed 50 characters");

        RuleFor(x => x.BusinessPartner)
            .NotNull()
            .WithMessage("Business partner information is required")
            .SetValidator(new BusinessPartnerInfoValidator());

        RuleFor(x => x.OperationalUnit)
            .NotNull()
            .WithMessage("Operational unit information is required")
            .SetValidator(new OperationalUnitInfoValidator());

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Transaction must have at least one item");

        RuleForEach(x => x.Items)
            .SetValidator(new TransactionItemInfoValidator());
    }
}

/// <summary>
/// Validator for BusinessPartnerInfo
/// </summary>
public class BusinessPartnerInfoValidator : AbstractValidator<BusinessPartnerInfo>
{
    public BusinessPartnerInfoValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Business partner external ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Business partner name is required")
            .MaximumLength(100)
            .WithMessage("Business partner name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Business partner email is required")
            .EmailAddress()
            .WithMessage("Business partner email must be valid");

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Business partner document is required");
    }
}

/// <summary>
/// Validator for OperationalUnitInfo
/// </summary>
public class OperationalUnitInfoValidator : AbstractValidator<OperationalUnitInfo>
{
    public OperationalUnitInfoValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Operational unit external ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Operational unit name is required")
            .MaximumLength(100)
            .WithMessage("Operational unit name cannot exceed 100 characters");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Operational unit location is required");
    }
}

/// <summary>
/// Validator for TransactionItemInfo
/// </summary>
public class TransactionItemInfoValidator : AbstractValidator<TransactionItemInfo>
{
    public TransactionItemInfoValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(x => x.ItemPrice)
            .GreaterThan(0)
            .WithMessage("Item price must be greater than zero");

        RuleFor(x => x.Product)
            .NotNull()
            .WithMessage("Product information is required")
            .SetValidator(new CommercialProductInfoValidator());
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