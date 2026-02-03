using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICommercialTransactionRepository using Entity Framework Core
/// </summary>
public class CommercialTransactionRepository : ICommercialTransactionRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of CommercialTransactionRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CommercialTransactionRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new commercial transaction in the database
    /// </summary>
    /// <param name="transaction">The transaction to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created transaction</returns>
    public async Task<CommercialTransaction> CreateAsync(CommercialTransaction transaction, CancellationToken cancellationToken = default)
    {
        await AddOrUpdateBusinessPartnerAsync(transaction.BusinessPartner, cancellationToken);
        await AddOrUpdateOperationalUnitAsync(transaction.OperationalUnit, cancellationToken);

        foreach (var item in transaction.Items)
        {
            if (item.Product is not null)
            {
                await AddOrUpdateCommercialProductAsync(item.Product, cancellationToken);
            }
        }

        await _context.CommercialTransactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction;
    }

    /// <summary>
    /// Retrieves a commercial transaction by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the transaction</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction if found, null otherwise</returns>
    public async Task<CommercialTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CommercialTransactions
            .Include(ct => ct.BusinessPartner)
            .Include(ct => ct.OperationalUnit)
            .Include(ct => ct.Items)
                .ThenInclude(ti => ti.Product)
            .FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a commercial transaction by its transaction code
    /// </summary>
    /// <param name="transactionCode">The transaction code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction if found, null otherwise</returns>
    public async Task<CommercialTransaction?> GetByTransactionCodeAsync(string transactionCode, CancellationToken cancellationToken = default)
    {
        return await _context.CommercialTransactions
            .Include(ct => ct.BusinessPartner)
            .Include(ct => ct.OperationalUnit)
            .Include(ct => ct.Items)
                .ThenInclude(ti => ti.Product)
            .FirstOrDefaultAsync(ct => ct.TransactionCode == transactionCode, cancellationToken);
    }

    /// <summary>
    /// Updates an existing commercial transaction
    /// </summary>
    /// <param name="transaction">The transaction to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated transaction</returns>
    public async Task<CommercialTransaction> UpdateAsync(CommercialTransaction transaction, CancellationToken cancellationToken = default)
    {
        _context.CommercialTransactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction;
    }

    /// <summary>
    /// Deletes a commercial transaction from the database
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the transaction was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await GetByIdAsync(id, cancellationToken);
        if (transaction is null)
            return false;

        _context.CommercialTransactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Retrieves all commercial transactions with pagination
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of transactions</returns>
    public async Task<IEnumerable<CommercialTransaction>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var skip = (page - 1) * size;

        return await _context.CommercialTransactions
            .Include(ct => ct.BusinessPartner)
            .Include(ct => ct.OperationalUnit)
            .Include(ct => ct.Items)
                .ThenInclude(ti => ti.Product)
            .OrderByDescending(ct => ct.CreatedAt)
            .Skip(skip)
            .Take(size)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds or updates a business partner using External Identity pattern
    /// </summary>
    /// <param name="businessPartner">The business partner to add or update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    private async Task AddOrUpdateBusinessPartnerAsync(BusinessPartner businessPartner, CancellationToken cancellationToken)
    {
        var existingPartner = await _context.BusinessPartners
            .FirstOrDefaultAsync(bp => bp.ExternalId == businessPartner.ExternalId, cancellationToken);

        if (existingPartner is null)
        {
            await _context.BusinessPartners.AddAsync(businessPartner, cancellationToken);
        }
        else
        {
            existingPartner.Name = businessPartner.Name;
            existingPartner.Email = businessPartner.Email;
            existingPartner.Document = businessPartner.Document;

            businessPartner.Id = existingPartner.Id;
            _context.Entry(businessPartner).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Adds or updates an operational unit using External Identity pattern
    /// </summary>
    /// <param name="operationalUnit">The operational unit to add or update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    private async Task AddOrUpdateOperationalUnitAsync(OperationalUnit operationalUnit, CancellationToken cancellationToken)
    {
        var existingUnit = await _context.OperationalUnits
            .FirstOrDefaultAsync(ou => ou.ExternalId == operationalUnit.ExternalId, cancellationToken);

        if (existingUnit is null)
        {
            await _context.OperationalUnits.AddAsync(operationalUnit, cancellationToken);
        }
        else
        {
            existingUnit.Name = operationalUnit.Name;
            existingUnit.Location = operationalUnit.Location;

            operationalUnit.Id = existingUnit.Id;
            _context.Entry(operationalUnit).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Adds or updates a commercial product using External Identity pattern
    /// </summary>
    /// <param name="product">The product to add or update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    private async Task AddOrUpdateCommercialProductAsync(Product product, CancellationToken cancellationToken)
    {
        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(cp => cp.ExternalId == product.ExternalId, cancellationToken);

        if (existingProduct is null)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }
        else
        {
            existingProduct.Name = product.Name;
            existingProduct.Category = product.Category;
            existingProduct.StandardPrice = product.StandardPrice;

            product.Id = existingProduct.Id;
            _context.Entry(product).State = EntityState.Detached;
        }
    }
}