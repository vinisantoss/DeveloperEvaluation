using Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;
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
    }
}
