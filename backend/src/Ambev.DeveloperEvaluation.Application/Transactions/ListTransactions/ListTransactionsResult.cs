namespace Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;

/// <summary>
/// Represents the response for listing commercial transactions
/// </summary>
public sealed class ListTransactionsResult
{
    /// <summary>
    /// Gets or sets the list of transactions
    /// </summary>
    public List<TransactionListItem> Transactions { get; set; } = new();

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total count of transactions
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Represents a transaction item in the list
/// </summary>
public class TransactionListItem
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
    /// Gets or sets the transaction date
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Gets or sets the business partner name
    /// </summary>
    public string BusinessPartnerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operational unit name
    /// </summary>
    public string OperationalUnitName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the grand total amount of the transaction
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// Gets or sets the transaction status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of items in the transaction
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// Gets or sets when the transaction was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}