using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;

/// <summary>
/// Command for cancelling a specific item in a commercial transaction
/// </summary>
public class CancelTransactionItemCommand : IRequest<CancelTransactionItemResult>
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
    /// Initializes a new CancelTransactionItemCommand
    /// </summary>
    /// <param name="transactionId">The transaction ID</param>
    /// <param name="itemId">The item ID</param>
    public CancelTransactionItemCommand(Guid transactionId, Guid itemId)
    {
        TransactionId = transactionId;
        ItemId = itemId;
    }
}