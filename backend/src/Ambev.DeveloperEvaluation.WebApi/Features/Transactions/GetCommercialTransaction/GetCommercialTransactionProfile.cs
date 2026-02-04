using Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;
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
    }
}
