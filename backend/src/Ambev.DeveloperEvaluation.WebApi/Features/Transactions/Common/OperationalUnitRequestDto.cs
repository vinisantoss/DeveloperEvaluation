namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;

/// <summary>
/// Operational unit information for requests
/// </summary>
public class OperationalUnitRequestDto
{
    /// <summary>
    /// External identifier of the operational unit
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Name of the operational unit
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Location of the operational unit
    /// </summary>
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Operational unit information for responses
/// </summary>
public class OperationalUnitResponseDto
{
    /// <summary>
    /// Unique identifier of the operational unit
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// External identifier of the operational unit
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Name of the operational unit
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Location of the operational unit
    /// </summary>
    public string Location { get; set; } = string.Empty;
}