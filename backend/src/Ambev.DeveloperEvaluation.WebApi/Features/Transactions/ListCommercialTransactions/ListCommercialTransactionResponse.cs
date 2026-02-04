namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.ListCommercialTransactions;

/// <summary>
/// API response model for ListTransactions operation
/// </summary>
public class ListTransactionsResponse
{
    /// <summary>
    /// List of transactions
    /// </summary>
    public List<TransactionSummary> Transactions { get; set; } = new();

    /// <summary>
    /// Total number of transactions found
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Transaction summary information for listing
/// </summary>
public class TransactionSummary
{
    /// <summary>
    /// The transaction's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The transaction code
    /// </summary>
    public string TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// The transaction date
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Business partner name
    /// </summary>
    public string BusinessPartnerName { get; set; } = string.Empty;

    /// <summary>
    /// Operational unit name
    /// </summary>
    public string OperationalUnitName { get; set; } = string.Empty;

    /// <summary>
    /// The grand total amount of the transaction
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// The transaction status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Number of items in the transaction
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// When the transaction was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}