using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransaction;

/// <summary>
/// AutoMapper profile for CancelTransaction operation mappings
/// </summary>
public sealed class CancelTransactionProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelTransaction
    /// </summary>
    public CancelTransactionProfile()
    {
        CreateMap<Guid, CancelTransactionCommand>()
            .ConstructUsing(id => new CancelTransactionCommand(id));

        CreateMap<CommercialTransaction, CancelTransactionResult>();
    }
}