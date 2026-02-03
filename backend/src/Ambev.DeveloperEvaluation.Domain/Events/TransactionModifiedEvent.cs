using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a commercial transaction is modified
/// </summary>
public class TransactionModifiedEvent
{
    public CommercialTransaction Transaction { get; }
    public string ModificationType { get; }
    public DateTime OccurredAt { get; }

    public TransactionModifiedEvent(CommercialTransaction transaction, string modificationType)
    {
        Transaction = transaction;
        ModificationType = modificationType;
        OccurredAt = DateTime.UtcNow;
    }
}