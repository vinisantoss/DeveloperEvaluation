using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for CommercialTransaction entity operations
/// </summary>
public interface ICommercialTransactionRepository : IRepository<CommercialTransaction>
{
    /// <summary>
    /// Retrieves a transaction by its unique identifier, eagerly loading BusinessPartner, OperationalUnit, and Items with Products.
    /// </summary>
    Task<CommercialTransaction?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a transaction by its transaction code
    /// </summary>
    /// <param name="transactionCode">The transaction code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction if found, null otherwise</returns>
    Task<CommercialTransaction?> GetByTransactionCodeAsync(string transactionCode, CancellationToken cancellationToken = default);
}