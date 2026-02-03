using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for CommercialTransaction entity operations
/// </summary>
public interface ICommercialTransactionRepository
{
    /// <summary>
    /// Creates a new commercial transaction in the repository
    /// </summary>
    /// <param name="transaction">The transaction to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created transaction</returns>
    Task<CommercialTransaction> CreateAsync(CommercialTransaction transaction, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a transaction by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the transaction</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction if found, null otherwise</returns>
    Task<CommercialTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a transaction by its transaction code
    /// </summary>
    /// <param name="transactionCode">The transaction code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction if found, null otherwise</returns>
    Task<CommercialTransaction?> GetByTransactionCodeAsync(string transactionCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing transaction
    /// </summary>
    /// <param name="transaction">The transaction to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated transaction</returns>
    Task<CommercialTransaction> UpdateAsync(CommercialTransaction transaction, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a transaction from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the transaction was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all transactions with pagination
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of transactions</returns>
    Task<IEnumerable<CommercialTransaction>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default);
}