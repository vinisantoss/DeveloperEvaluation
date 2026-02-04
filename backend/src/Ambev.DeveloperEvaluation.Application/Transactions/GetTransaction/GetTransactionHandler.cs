using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;

/// <summary>
/// Handler for processing GetTransactionCommand requests
/// </summary>
public class GetTransactionHandler : IRequestHandler<GetTransactionCommand, GetTransactionResult>
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTransactionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetTransactionHandler
    /// </summary>
    /// <param name="transactionRepository">The transaction repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public GetTransactionHandler(
        ICommercialTransactionRepository transactionRepository,
        IMapper mapper,
        ILogger<GetTransactionHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetTransactionCommand request
    /// </summary>
    /// <param name="request">The GetTransaction command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The transaction details</returns>
    public async Task<GetTransactionResult> Handle(GetTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving commercial transaction with ID {TransactionId}", request.Id);

        var validator = new GetTransactionCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction == null)
        {
            throw new InvalidOperationException($"Commercial transaction with ID {request.Id} not found");
        }

        _logger.LogInformation("Commercial transaction retrieved successfully with ID {TransactionId}", transaction.Id);

        return _mapper.Map<GetTransactionResult>(transaction);
    }
}