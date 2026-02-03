using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a commercial transaction is cancelled
/// </summary>
public class TransactionCancelledEvent
{
    public CommercialTransaction Transaction { get; }
    public DateTime OccurredAt { get; }

    public TransactionCancelledEvent(CommercialTransaction transaction)
    {
        Transaction = transaction;
        OccurredAt = DateTime.UtcNow;
    }
}