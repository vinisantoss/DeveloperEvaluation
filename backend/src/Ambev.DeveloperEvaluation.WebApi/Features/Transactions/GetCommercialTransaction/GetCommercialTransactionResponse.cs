using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CreateCommercialTransaction;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.GetCommercialTransaction;

/// <summary>
/// API response model for GetTransaction operation
/// </summary>
public class GetTransactionResponse
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
    /// Business partner information
    /// </summary>
    public BusinessPartnerResponseDto BusinessPartner { get; set; } = new();

    /// <summary>
    /// Operational unit information
    /// </summary>
    public OperationalUnitResponseDto OperationalUnit { get; set; } = new();

    /// <summary>
    /// The grand total amount of the transaction
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// The transaction status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// List of items in the transaction
    /// </summary>
    public List<TransactionItemResponseDto> Items { get; set; } = new();

    /// <summary>
    /// When the transaction was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the transaction was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Business partner information
/// </summary>
public class BusinessPartnerInfo
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Operational unit information
/// </summary>
public class OperationalUnitInfo
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Transaction item information
/// </summary>
public class TransactionItemInfo
{
    public Guid Id { get; set; }
    public ProductInfo Product { get; set; } = new();
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal ItemTotal { get; set; }
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Product information
/// </summary>
public class ProductInfo
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}