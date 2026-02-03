using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a transaction item is cancelled
/// </summary>
public class TransactionItemCancelledEvent
{
    public TransactionItem Item { get; }
    public CommercialTransaction Transaction { get; }
    public DateTime OccurredAt { get; }

    public TransactionItemCancelledEvent(TransactionItem item, CommercialTransaction transaction)
    {
        Item = item;
        Transaction = transaction;
        OccurredAt = DateTime.UtcNow;
    }
}