using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a business partner using External Identity pattern.
/// </summary>
public class BusinessPartner : BaseEntity
{
    /// <summary>
    /// External identifier from the BusinessPartner
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Business partner name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Business partner email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Business partner document.
    /// </summary>
    public string Document { get; set; } = string.Empty;
}