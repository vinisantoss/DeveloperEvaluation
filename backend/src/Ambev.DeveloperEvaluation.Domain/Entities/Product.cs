using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product using External Identity pattern.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// External identifier from the Product domain
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Product category
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Product standard price
    /// </summary>
    public decimal StandardPrice { get; set; }
}