using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Transactions.UpdateTransaction;

/// <summary>
/// Handler for processing UpdateTransactionCommand requests
/// </summary>
public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, UpdateTransactionResult>
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateTransactionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateTransactionHandler
    /// </summary>
    /// <param name="transactionRepository">The transaction repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public UpdateTransactionHandler(
        ICommercialTransactionRepository transactionRepository,
        IMapper mapper,
        ILogger<UpdateTransactionHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the UpdateTransactionCommand request
    /// </summary>
    /// <param name="request">The UpdateTransaction command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated transaction details</returns>
    public async Task<UpdateTransactionResult> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating commercial transaction with ID {TransactionId}", request.Id);

        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null)
        {
            throw new InvalidOperationException($"Commercial transaction with ID {request.Id} not found");
        }

        foreach (var itemInfo in request.Items)
        {
            switch (itemInfo.Operation)
            {
                case UpdateOperation.Add:
                    await AddNewItem(transaction, itemInfo);
                    break;

                case UpdateOperation.Update:
                    UpdateExistingItem(transaction, itemInfo);
                    break;

                case UpdateOperation.Remove:
                    RemoveItem(transaction, itemInfo);
                    break;
            }
        }

        var validationResult = transaction.Validate();
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.Detail));
            throw new InvalidOperationException($"Transaction validation failed: {errors}");
        }

        var updatedTransaction = await _transactionRepository.UpdateAsync(transaction, cancellationToken);

        var transactionModifiedEvent = new TransactionModifiedEvent(updatedTransaction, "ItemsUpdated");
        _logger.LogInformation("TransactionModified event: Transaction {TransactionId} modified at {ModifiedAt} - {ModificationType}",
            transactionModifiedEvent.Transaction.Id,
            transactionModifiedEvent.OccurredAt,
            transactionModifiedEvent.ModificationType);

        _logger.LogInformation("Commercial transaction updated successfully with ID {TransactionId}", updatedTransaction.Id);

        return _mapper.Map<UpdateTransactionResult>(updatedTransaction);
    }

    private async Task AddNewItem(CommercialTransaction transaction, UpdateTransactionItemInfo itemInfo)
    {
        if (itemInfo.Product is null || itemInfo.ItemPrice is null)
        {
            throw new InvalidOperationException("Product information and item price are required for adding new items");
        }

        var product = new Product
        {
            ExternalId = itemInfo.Product.ExternalId,
            Name = itemInfo.Product.Name,
            Category = itemInfo.Product.Category,
            StandardPrice = itemInfo.Product.StandardPrice
        };

        transaction.AddItem(product.Id, itemInfo.Quantity, itemInfo.ItemPrice.Value);

        var lastItem = transaction.Items.Last();
        lastItem.Product = product;
    }

    private void UpdateExistingItem(CommercialTransaction transaction, UpdateTransactionItemInfo itemInfo)
    {
        if (itemInfo.ItemId is null)
        {
            throw new InvalidOperationException("Item ID is required for updating existing items");
        }

        transaction.UpdateItemQuantity(itemInfo.ItemId.Value, itemInfo.Quantity);
    }

    private void RemoveItem(CommercialTransaction transaction, UpdateTransactionItemInfo itemInfo)
    {
        if (itemInfo.ItemId is null)
        {
            throw new InvalidOperationException("Item ID is required for removing items");
        }

        transaction.CancelItem(itemInfo.ItemId.Value);
    }
}