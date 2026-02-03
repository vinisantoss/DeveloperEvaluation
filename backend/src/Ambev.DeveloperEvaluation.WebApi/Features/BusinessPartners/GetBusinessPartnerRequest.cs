namespace Ambev.DeveloperEvaluation.WebApi.Features.BusinessPartners;

/// <summary>
/// Request model for getting a business partner by ID
/// </summary>
public class GetBusinessPartnerRequest
{
    /// <summary>
    /// The unique identifier of the business partner
    /// </summary>
    public Guid Id { get; set; }
}