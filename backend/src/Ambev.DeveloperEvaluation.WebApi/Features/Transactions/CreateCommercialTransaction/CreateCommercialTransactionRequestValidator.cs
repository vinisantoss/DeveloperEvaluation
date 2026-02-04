using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CreateCommercialTransaction;

/// <summary>
/// Validator for CreateCommercialTransactionRequest
/// </summary>
public class CreateCommercialTransactionRequestValidator : AbstractValidator<CreateCommercialTransactionRequest>
{
    public CreateCommercialTransactionRequestValidator()
    {
        RuleFor(x => x.TransactionCode)
            .NotEmpty()
            .WithMessage("Transaction code is required")
            .MaximumLength(50)
            .WithMessage("Transaction code cannot exceed 50 characters");

        RuleFor(x => x.BusinessPartner)
            .NotNull()
            .WithMessage("Business partner information is required")
            .SetValidator(new BusinessPartnerRequestDtoValidator());

        RuleFor(x => x.OperationalUnit)
            .NotNull()
            .WithMessage("Operational unit information is required")
            .SetValidator(new OperationalUnitRequestDtoValidator());

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Transaction must have at least one item");

        RuleForEach(x => x.Items)
            .SetValidator(new TransactionItemRequestDtoValidator());
    }
}

/// <summary>
/// Validator for BusinessPartnerRequestDto
/// </summary>
public class BusinessPartnerRequestDtoValidator : AbstractValidator<BusinessPartnerRequestDto>
{
    public BusinessPartnerRequestDtoValidator()
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
/// Validator for OperationalUnitRequestDto
/// </summary>
public class OperationalUnitRequestDtoValidator : AbstractValidator<OperationalUnitRequestDto>
{
    public OperationalUnitRequestDtoValidator()
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
/// Validator for TransactionItemRequestDto
/// </summary>
public class TransactionItemRequestDtoValidator : AbstractValidator<TransactionItemRequestDto>
{
    public TransactionItemRequestDtoValidator()
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
            .SetValidator(new ProductRequestDtoValidator());
    }
}

/// <summary>
/// Validator for ProductRequestDto
/// </summary>
public class ProductRequestDtoValidator : AbstractValidator<ProductRequestDto>
{
    public ProductRequestDtoValidator()
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