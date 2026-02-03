using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;

/// <summary>
/// Handler for processing CancelTransactionItemCommand requests
/// </summary>
public class CancelTransactionItemHandler : IRequestHandler<CancelTransactionItemCommand, CancelTransactionItemResult>
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly ILogger<CancelTransactionItemHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelTransactionItemHandler
    /// </summary>
    /// <param name="transactionRepository">The transaction repository</param>
    /// <param name="logger">The logger</param>
    public CancelTransactionItemHandler(
        ICommercialTransactionRepository transactionRepository,
        ILogger<CancelTransactionItemHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CancelTransactionItemCommand request
    /// </summary>
    /// <param name="request">The CancelTransactionItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cancellation result</returns>
    public async Task<CancelTransactionItemResult> Handle(CancelTransactionItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling item {ItemId} from transaction {TransactionId}", request.ItemId, request.TransactionId);

        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
        if (transaction is null)
        {
            throw new InvalidOperationException($"Commercial transaction with ID {request.TransactionId} not found");
        }

        var item = transaction.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item == null)
        {
            throw new InvalidOperationException($"Item with ID {request.ItemId} not found in transaction {request.TransactionId}");
        }

        transaction.CancelItem(request.ItemId);

        var updatedTransaction = await _transactionRepository.UpdateAsync(transaction, cancellationToken);

        var itemCancelledEvent = new TransactionItemCancelledEvent(item, updatedTransaction);
        _logger.LogInformation("TransactionItemCancelled event: Item {ItemId} from transaction {TransactionId} cancelled at {CancelledAt}",
            itemCancelledEvent.Item.Id,
            itemCancelledEvent.Transaction.Id,
            itemCancelledEvent.OccurredAt);

        _logger.LogInformation("Transaction item cancelled successfully - Transaction: {TransactionId}, Item: {ItemId}", request.TransactionId, request.ItemId);

        return new CancelTransactionItemResult
        {
            TransactionId = updatedTransaction.Id,
            ItemId = request.ItemId,
            TransactionCode = updatedTransaction.TransactionCode,
            UpdatedGrandTotal = updatedTransaction.Amount,
            UpdatedAt = updatedTransaction.UpdatedAt,
            Message = $"Item {request.ItemId} has been successfully cancelled from transaction {updatedTransaction.TransactionCode}"
        };
    }
}