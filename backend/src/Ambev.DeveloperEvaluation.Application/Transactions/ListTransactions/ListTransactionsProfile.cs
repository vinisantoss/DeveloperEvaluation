using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;

/// <summary>
/// AutoMapper profile for ListTransactions operation mappings
/// </summary>
public sealed class ListTransactionsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListTransactions
    /// </summary>
    public ListTransactionsProfile()
    {
        CreateMap<CommercialTransaction, TransactionListItem>()
            .ForMember(dest => dest.BusinessPartnerName, opt => opt.MapFrom(src => src.BusinessPartner.Name))
            .ForMember(dest => dest.OperationalUnitName, opt => opt.MapFrom(src => src.OperationalUnit.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Items.Count(i => !i.IsCancelled)));
    }
}