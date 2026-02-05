
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Generic repository interface for common entity operations
/// </summary>
/// <typeparam name="TEntity">The type of the entity, must inherit from BaseEntity</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Creates a new entity in the repository
    /// </summary>
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an entity by its unique identifier
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity from the repository
    /// </summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities with pagination
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default);
}