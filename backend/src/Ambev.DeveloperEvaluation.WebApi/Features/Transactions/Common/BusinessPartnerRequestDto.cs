namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;

/// <summary>
/// Business partner information for requests
/// </summary>
public class BusinessPartnerRequestDto
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Business partner information for responses
/// </summary>
public class BusinessPartnerResponseDto
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}