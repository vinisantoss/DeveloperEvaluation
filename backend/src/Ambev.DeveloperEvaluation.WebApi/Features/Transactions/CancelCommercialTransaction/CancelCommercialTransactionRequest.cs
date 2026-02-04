namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransaction;

/// <summary>
/// Request model for cancelling a transaction
/// </summary>
public class CancelTransactionRequest
{
    /// <summary>
    /// The unique identifier of the transaction to cancel
    /// </summary>
    public Guid Id { get; set; }
}