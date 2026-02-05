using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class OperationalUnitRepository : Repository<OperationalUnit>, IOperationalUnitRepository
{
    public OperationalUnitRepository(DefaultContext context) : base(context) { }

    public async Task<OperationalUnit?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(ou => ou.ExternalId == externalId, cancellationToken);
    }
}
