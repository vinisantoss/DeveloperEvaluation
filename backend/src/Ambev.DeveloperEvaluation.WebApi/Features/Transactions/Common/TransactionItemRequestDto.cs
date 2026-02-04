namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;

/// <summary>
/// Transaction item information for requests
/// </summary>
public class TransactionItemRequestDto
{
    /// <summary>
    /// Product information
    /// </summary>
    public ProductRequestDto Product { get; set; } = new();

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price of the item
    /// </summary>
    public decimal ItemPrice { get; set; }
}

/// <summary>
/// Transaction item information for responses
/// </summary>
public class TransactionItemResponseDto
{
    /// <summary>
    /// Unique identifier of the transaction item
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product information
    /// </summary>
    public ProductResponseDto Product { get; set; } = new();

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price of the item
    /// </summary>
    public decimal ItemPrice { get; set; }

    /// <summary>
    /// Discount percentage applied to the item
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// Total amount for this item
    /// </summary>
    public decimal ItemTotal { get; set; }

    /// <summary>
    /// Indicates if the item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}