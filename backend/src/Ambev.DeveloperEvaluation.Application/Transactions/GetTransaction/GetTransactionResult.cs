namespace Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;

/// <summary>
/// Represents the response for getting a commercial transaction
/// </summary>
public sealed class GetTransactionResult
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
    /// Gets or sets the business partner information
    /// </summary>
    public BusinessPartnerResult BusinessPartner { get; set; } = new();

    /// <summary>
    /// Gets or sets the operational unit information
    /// </summary>
    public OperationalUnitResult OperationalUnit { get; set; } = new();

    /// <summary>
    /// Gets or sets the grand total amount of the transaction
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// Gets or sets the transaction status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items in the transaction
    /// </summary>
    public List<TransactionItemResult> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets when the transaction was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the transaction was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Business partner result information
/// </summary>
public class BusinessPartnerResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Operational unit result information
/// </summary>
public class OperationalUnitResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Transaction item result information
/// </summary>
public class TransactionItemResult
{
    public Guid Id { get; set; }
    public CommercialProductResult Product { get; set; } = new();
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal ItemTotal { get; set; }
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Commercial product result information
/// </summary>
public class CommercialProductResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}