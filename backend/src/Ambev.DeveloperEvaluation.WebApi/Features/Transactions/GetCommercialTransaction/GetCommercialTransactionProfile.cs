using Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.GetCommercialTransaction;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.ListCommercialTransactions;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.GetTransaction;

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
        CreateMap<Guid, GetTransactionCommand>()
            .ConstructUsing(id => new GetTransactionCommand(id));

        CreateMap<ListTransactionsResult, ListTransactionsResponse>();
        //CreateMap<TransactionItemInfo, >();
        CreateMap<GetTransactionResult, GetTransactionResponse>();
    }
}
