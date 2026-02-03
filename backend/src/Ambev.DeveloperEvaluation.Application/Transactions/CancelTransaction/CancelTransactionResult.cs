namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;

/// <summary>
/// Represents the response after cancelling a commercial transaction
/// </summary>
public sealed class CancelTransactionResult
{
    /// <summary>
    /// Gets or sets the transaction's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the transaction code
    /// </summary>
    public string TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the transaction status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when the transaction was cancelled
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets a success message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}