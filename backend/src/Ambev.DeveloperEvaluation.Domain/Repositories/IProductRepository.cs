using Ambev.DeveloperEvaluation.Domain.Entities;


namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Product entity operations
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Retrieves a product by its external identifier
    /// </summary>
    Task<Product?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
}