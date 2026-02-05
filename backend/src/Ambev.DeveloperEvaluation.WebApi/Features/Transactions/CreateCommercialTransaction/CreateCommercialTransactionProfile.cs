using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.Common; 
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
        CreateMap<TransactionItemRequestDto, TransactionItemInfo>();
        CreateMap<CreateCommercialTransactionRequest, CreateCommercialTransactionCommand>();
        CreateMap<CreateCommercialTransactionResult, CreateCommercialTransactionResponse>();
        CreateMap<BusinessPartnerRequestDto, BusinessPartnerInfo>();
        CreateMap<OperationalUnitRequestDto, OperationalUnitInfo>();
        CreateMap<TransactionItemRequestDto, TransactionItemResponseInfo>();
        CreateMap<ProductRequestDto, ProductResponseInfo>();
        CreateMap<ProductRequestDto,CommercialProductInfo>();
        CreateMap<BusinessPartnerResult,BusinessPartnerResponseDto>();
        CreateMap<OperationalUnitResult, OperationalUnitResponseDto>();
        CreateMap<ProductResult, ProductResponseDto>();
        CreateMap<TransactionItemResult, TransactionItemRequestDto>();
        CreateMap<ProductResult,ProductRequestDto>();
    }
}