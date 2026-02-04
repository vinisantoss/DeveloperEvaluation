namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.ListCommercialTransactions;

/// <summary>
/// Request model for listing transactions with filtering options
/// </summary>
public class ListTransactionsRequest
{
    /// <summary>
    /// Page number for pagination (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10, max: 100)
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Filter by transaction code
    /// </summary>
    public string? TransactionCode { get; set; }

    /// <summary>
    /// Filter by business partner external ID
    /// </summary>
    public string? BusinessPartnerExternalId { get; set; }

    /// <summary>
    /// Filter by operational unit external ID
    /// </summary>
    public string? OperationalUnitExternalId { get; set; }

    /// <summary>
    /// Filter by transaction status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by start date (inclusive)
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Filter by end date (inclusive)
    /// </summary>
    public DateTime? EndDate { get; set; }
}