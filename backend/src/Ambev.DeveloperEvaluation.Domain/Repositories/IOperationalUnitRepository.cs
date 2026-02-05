using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for OperationalUnit entity operations
/// </summary>
public interface IOperationalUnitRepository : IRepository<OperationalUnit>
{
    /// <summary>
    /// Retrieves an operational unit by its external identifier
    /// </summary>
    Task<OperationalUnit?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
}