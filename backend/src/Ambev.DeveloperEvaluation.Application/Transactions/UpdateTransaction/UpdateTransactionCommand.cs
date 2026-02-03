using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Transactions.UpdateTransaction;

/// <summary>
/// Command for updating an existing commercial transaction
/// </summary>
public class UpdateTransactionCommand : IRequest<UpdateTransactionResult>
{
    /// <summary>
    /// Gets or sets the transaction's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the list of items to update in the transaction
    /// </summary>
    public List<UpdateTransactionItemInfo> Items { get; set; } = new();

    /// <summary>
    /// Validates the command using the validator
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new UpdateTransactionCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

/// <summary>
/// Information for updating a transaction item
/// </summary>
public class UpdateTransactionItemInfo
{
    /// <summary>
    /// Item ID (if updating existing item)
    /// </summary>
    public Guid? ItemId { get; set; }

    /// <summary>
    /// Product information (if adding new item)
    /// </summary>
    public CommercialProductInfo? Product { get; set; }

    /// <summary>
    /// New quantity for the item
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Item price (for new items)
    /// </summary>
    public decimal? ItemPrice { get; set; }

    /// <summary>
    /// Operation type: Add, Update, or Remove
    /// </summary>
    public UpdateOperation Operation { get; set; }
}

/// <summary>
/// Commercial product information for new items
/// </summary>
public class CommercialProductInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}

/// <summary>
/// Types of update operations
/// </summary>
public enum UpdateOperation
{
    Add,
    Update,
    Remove
}