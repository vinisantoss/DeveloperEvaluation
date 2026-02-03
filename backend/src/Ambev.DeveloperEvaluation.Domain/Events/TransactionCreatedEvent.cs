using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a new commercial transaction is created
/// </summary>
public class TransactionCreatedEvent
{
    public CommercialTransaction Transaction { get; }
    public DateTime OccurredAt { get; }

    public TransactionCreatedEvent(CommercialTransaction transaction)
    {
        Transaction = transaction;
        OccurredAt = DateTime.UtcNow;
    }
}