using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;

/// <summary>
/// Handler for processing CancelTransactionCommand requests
/// </summary>
public class CancelTransactionHandler : IRequestHandler<CancelTransactionCommand, CancelTransactionResult>
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelTransactionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelTransactionHandler
    /// </summary>
    /// <param name="transactionRepository">The transaction repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public CancelTransactionHandler(
        ICommercialTransactionRepository transactionRepository,
        IMapper mapper,
        ILogger<CancelTransactionHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CancelTransactionCommand request
    /// </summary>
    /// <param name="request">The CancelTransaction command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cancellation result</returns>
    public async Task<CancelTransactionResult> Handle(CancelTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling commercial transaction with ID {TransactionId}", request.Id);

        var validator = new CancelTransactionCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null)
        {
            throw new InvalidOperationException($"Commercial transaction with ID {request.Id} not found");
        }

        transaction.Cancel();

        var updatedTransaction = await _transactionRepository.UpdateAsync(transaction, cancellationToken);

        var transactionCancelledEvent = new TransactionCancelledEvent(updatedTransaction);
        _logger.LogInformation("TransactionCancelled event: Transaction {TransactionId} with code {TransactionCode} cancelled at {CancelledAt}",
            transactionCancelledEvent.Transaction.Id,
            transactionCancelledEvent.Transaction.TransactionCode,
            transactionCancelledEvent.OccurredAt);

        _logger.LogInformation("Commercial transaction cancelled successfully with ID {TransactionId}", updatedTransaction.Id);

        return _mapper.Map<CancelTransactionResult>(updatedTransaction);
    }
}