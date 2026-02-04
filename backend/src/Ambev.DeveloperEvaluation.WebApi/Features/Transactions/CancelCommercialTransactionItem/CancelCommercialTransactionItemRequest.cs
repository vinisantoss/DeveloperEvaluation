namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransactionItem;

/// <summary>
/// Request model for cancelling a transaction item
/// </summary>
public class CancelTransactionItemRequest
{
    /// <summary>
    /// The unique identifier of the transaction
    /// </summary>
    public Guid TransactionId { get; set; }

    /// <summary>
    /// The unique identifier of the item to cancel
    /// </summary>
    public Guid ItemId { get; set; }
}