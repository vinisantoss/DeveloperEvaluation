using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Transactions.UpdateTransaction;

/// <summary>
/// AutoMapper profile for UpdateTransaction operation mappings
/// </summary>
public sealed class UpdateTransactionProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateTransaction
    /// </summary>
    public UpdateTransactionProfile()
    {
        CreateMap<CommercialTransaction, UpdateTransactionResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<BusinessPartner, BusinessPartnerResult>();
        CreateMap<OperationalUnit, OperationalUnitResult>();
        CreateMap<TransactionItem, TransactionItemResult>();
        CreateMap<Product, ProductResult>();
    }
}