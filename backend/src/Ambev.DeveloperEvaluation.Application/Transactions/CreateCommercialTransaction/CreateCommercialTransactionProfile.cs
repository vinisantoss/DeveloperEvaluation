using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;

/// <summary>
/// AutoMapper profile for CreateCommercialTransaction operation mappings
/// </summary>
public sealed class CreateCommercialTransactionProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCommercialTransaction
    /// </summary>
    public CreateCommercialTransactionProfile()
    {
        CreateMap<CommercialTransaction, CreateCommercialTransactionResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<BusinessPartner, BusinessPartnerResult>();
        CreateMap<OperationalUnit, OperationalUnitResult>();
        CreateMap<TransactionItem, TransactionItemResult>();
        CreateMap<Product, ProductResult>();
    }
}