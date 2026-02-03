using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;

/// <summary>
/// AutoMapper profile for GetTransaction operation mappings
/// </summary>
public sealed class GetTransactionProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetTransaction
    /// </summary>
    public GetTransactionProfile()
    {
        CreateMap<CommercialTransaction, GetTransactionResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<BusinessPartner, BusinessPartnerResult>();
        CreateMap<OperationalUnit, OperationalUnitResult>();
        CreateMap<TransactionItem, TransactionItemResult>();
        CreateMap<Product, CommercialProductResult>();
    }
}