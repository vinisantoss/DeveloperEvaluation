using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICommercialTransactionRepository using Entity Framework Core
/// </summary>
public class CommercialTransactionRepository : Repository<CommercialTransaction>, ICommercialTransactionRepository
{
    /// <summary>
    /// Initializes a new instance of CommercialTransactionRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CommercialTransactionRepository(DefaultContext context) : base(context) { }

    public async Task<CommercialTransaction?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
     => await _dbSet
                     .Include(ct => ct.BusinessPartner)
                     .Include(ct => ct.OperationalUnit)
                     .Include(ct => ct.Items)
                         .ThenInclude(item => item.Product)
                     .FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);

    /// <summary>
    /// Retrieves a commercial transaction by its transaction code
    /// </summary>
    /// <param name="transactionCode">The transaction code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction if found, null otherwise</returns>
    public async Task<CommercialTransaction?> GetByTransactionCodeAsync(string transactionCode, CancellationToken cancellationToken = default)
     =>
        await _dbSet
                     .Include(ct => ct.BusinessPartner)
                     .Include(ct => ct.OperationalUnit)
                     .Include(ct => ct.Items)
                         .ThenInclude(item => item.Product)
                     .FirstOrDefaultAsync(ct => ct.TransactionCode == transactionCode, cancellationToken);
}