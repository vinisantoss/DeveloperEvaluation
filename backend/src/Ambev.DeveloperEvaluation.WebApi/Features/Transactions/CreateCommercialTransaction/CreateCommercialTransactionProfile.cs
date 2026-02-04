using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CreateCommercialTransaction;

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
        CreateMap<CreateCommercialTransactionRequest, CreateCommercialTransactionCommand>();
        CreateMap<CreateCommercialTransactionResult, CreateCommercialTransactionResponse>();
    }
}
