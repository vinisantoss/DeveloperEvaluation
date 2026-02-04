using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransactionItem;

/// <summary>
/// AutoMapper profile for CancelTransactionItem operation mappings
/// </summary>
public sealed class CancelTransactionItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelTransactionItem
    /// </summary>
    public CancelTransactionItemProfile()
    {
        CreateMap<CancelTransactionItemRequest, CancelTransactionItemCommand>()
           .ConstructUsing(src => new CancelTransactionItemCommand(src.TransactionId, src.ItemId));
    }
}