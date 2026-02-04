using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransaction;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CancelCommercialTransactionItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.CreateCommercialTransaction;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.GetCommercialTransaction;
using Ambev.DeveloperEvaluation.WebApi.Features.Transactions.ListCommercialTransactions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Transactions;

/// <summary>
/// Controller for managing commercial transaction operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of TransactionsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public TransactionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new commercial transaction
    /// </summary>
    /// <param name="request">The transaction creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created transaction details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCommercialTransactionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateCommercialTransactionRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCommercialTransactionRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCommercialTransactionCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateCommercialTransactionResponse>
        {
            Success = true,
            Message = "Transaction created successfully",
            Data = _mapper.Map<CreateCommercialTransactionResponse>(response)
        });
    }

    /// <summary>
    /// Retrieves a transaction by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the transaction</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetTransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransaction([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCommercialTransactionRequest { Id = id };
        var validator = new GetTransactionRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetTransactionCommand>(request.Id);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<GetTransactionResponse>
        {
            Success = true,
            Message = "Transaction retrieved successfully",
            Data = _mapper.Map<GetTransactionResponse>(response)
        });
    }

    /// <summary>
    /// Lists transactions with optional filtering
    /// </summary>
    /// <param name="request">The list transactions request with filters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of transactions</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListTransactionsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListTransactions([FromQuery] ListTransactionsRequest request, CancellationToken cancellationToken)
    {
        var validator = new ListTransactionsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<ListTransactionsCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<ListTransactionsResponse>
        {
            Success = true,
            Message = "Transactions retrieved successfully",
            Data = _mapper.Map<ListTransactionsResponse>(response)
        });
    }

    /// <summary>
    /// Cancels a complete transaction
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the transaction was cancelled</returns>
    [HttpDelete("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTransaction([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new CancelTransactionRequest { Id = id };
        var validator = new CancelTransactionRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CancelTransactionCommand>(request.Id);
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Transaction cancelled successfully"
        });
    }

    /// <summary>
    /// Cancels a specific item from a transaction
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction</param>
    /// <param name="itemId">The unique identifier of the item to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the item was cancelled</returns>
    [HttpDelete("{transactionId}/items/{itemId}/cancel")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTransactionItem([FromRoute] Guid transactionId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
    {
        var request = new CancelTransactionItemRequest
        {
            TransactionId = transactionId,
            ItemId = itemId
        };

        var validator = new CancelTransactionItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CancelTransactionItemCommand>(request);
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Transaction item cancelled successfully"
        });
    }
}