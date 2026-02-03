namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;

/// <summary>
/// Represents the response after cancelling a transaction item
/// </summary>
public sealed class CancelTransactionItemResult
{
    /// <summary>
    /// Gets or sets the transaction's unique identifier
    /// </summary>
    public Guid TransactionId { get; set; }

    /// <summary>
    /// Gets or sets the item's unique identifier
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets the transaction code
    /// </summary>
    public string TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated grand total amount of the transaction
    /// </summary>
    public decimal UpdatedGrandTotal { get; set; }

    /// <summary>
    /// Gets or sets when the item was cancelled
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets a success message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}