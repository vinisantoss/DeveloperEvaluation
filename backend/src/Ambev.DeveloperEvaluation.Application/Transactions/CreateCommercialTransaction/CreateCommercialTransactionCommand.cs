using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;

/// <summary>
/// Command for creating a new commercial transaction.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a transaction, 
/// including transaction code, business partner, operational unit, and items. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CreateCommercialTransactionResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateCommercialTransactionCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class CreateCommercialTransactionCommand : IRequest<CreateCommercialTransactionResult>
{
    /// <summary>
    /// Gets or sets the transaction code
    /// </summary>
    public string TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the business partner information
    /// </summary>
    public BusinessPartnerInfo BusinessPartner { get; set; } = new();

    /// <summary>
    /// Gets or sets the operational unit information
    /// </summary>
    public OperationalUnitInfo OperationalUnit { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of items in the transaction
    /// </summary>
    public List<TransactionItemInfo> Items { get; set; } = new();

    /// <summary>
    /// Validates the command using the validator
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CreateCommercialTransactionCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

/// <summary>
/// Business partner information for External Identity pattern
/// </summary>
public class BusinessPartnerInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Operational unit information for External Identity pattern
/// </summary>
public class OperationalUnitInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Transaction item information
/// </summary>
public class TransactionItemInfo
{
    public CommercialProductInfo Product { get; set; } = new();
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
}

/// <summary>
/// Commercial product information for External Identity pattern
/// </summary>
public class CommercialProductInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}