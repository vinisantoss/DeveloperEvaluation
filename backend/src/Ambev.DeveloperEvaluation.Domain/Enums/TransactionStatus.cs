namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents the possible statuses for a commercial transaction
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Transaction is active and valid
    /// </summary>
    Active = 1,

    /// <summary>
    /// Transaction has been cancelled
    /// </summary>
    Cancelled = 2
}