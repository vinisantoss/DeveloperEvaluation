using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class BusinessPartnerRepository : Repository<BusinessPartner>, IBusinessPartnerRepository
{
    public BusinessPartnerRepository(DefaultContext context) : base(context) { }

    public async Task<BusinessPartner?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(bp => bp.ExternalId == externalId, cancellationToken);
    }
}
