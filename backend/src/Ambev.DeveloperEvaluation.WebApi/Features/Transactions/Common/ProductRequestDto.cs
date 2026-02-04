namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;

/// <summary>
/// Product information for requests
/// </summary>
public class ProductRequestDto
{
    /// <summary>
    /// External identifier of the product
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category of the product
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Standard price of the product
    /// </summary>
    public decimal StandardPrice { get; set; }
}

/// <summary>
/// Product information for responses
/// </summary>
public class ProductResponseDto
{
    /// <summary>
    /// Unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// External identifier of the product
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category of the product
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Standard price of the product
    /// </summary>
    public decimal StandardPrice { get; set; }
}