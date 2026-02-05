using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CreateCommercialTransaction;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.ListCommercialTransactions;

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
        CreateMap<ListTransactionsRequest, ListTransactionsCommand>()
            .ConstructUsing(src => new ListTransactionsCommand(src.Page, src.Size));

        CreateMap<ListTransactionsResult, ListTransactionsResponse>();
        CreateMap<TransactionListItem, TransactionSummary>();

    }
}
