namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.GetCommercialTransaction;

/// <summary>
/// Request model for getting a transaction by ID
/// </summary>
public class GetCommercialTransactionRequest
{
    /// <summary>
    /// The unique identifier of the transaction to retrieve
    /// </summary>
    public Guid Id { get; set; }
}