using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CreateCommercialTransaction;

/// <summary>
/// Request model for creating a commercial transaction
/// </summary>
public class CreateCommercialTransactionRequest
{
    /// <summary>
    /// The transaction code
    /// </summary>
    public string TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// Business partner information
    /// </summary>
    public BusinessPartnerRequestDto BusinessPartner { get; set; } = new();

    /// <summary>
    /// Operational unit information
    /// </summary>
    public OperationalUnitRequestDto OperationalUnit { get; set; } = new();

    /// <summary>
    /// List of items in the transaction
    /// </summary>
    public List<TransactionItemRequestDto> Items { get; set; } = new();
}

/// <summary>
/// Business partner information for request
/// </summary>
public class BusinessPartnerRequestInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Operational unit information for request
/// </summary>
public class OperationalUnitRequestInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Transaction item information for request
/// </summary>
public class TransactionItemRequestInfo
{
    public CommercialProductRequestInfo Product { get; set; } = new();
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
}

/// <summary>
/// Commercial product information for request
/// </summary>
public class CommercialProductRequestInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}