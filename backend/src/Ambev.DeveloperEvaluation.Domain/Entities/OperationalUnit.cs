using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an operational unit using External Identity pattern
/// </summary>
public class OperationalUnit : BaseEntity
{
    /// <summary>
    /// External identifier from the OperationalUnit
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// OperationalUnit name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// OperationalUnit location
    /// </summary>
    public string Location { get; set; } = string.Empty;
}