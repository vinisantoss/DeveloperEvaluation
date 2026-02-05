using Ambev.DeveloperEvaluation.Domain.Entities;
namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for BusinessPartner entity operations
/// </summary>
public interface IBusinessPartnerRepository : IRepository<BusinessPartner>
{
    /// <summary>
    /// Retrieves a business partner by its external identifier
    /// </summary>
    Task<BusinessPartner?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
}