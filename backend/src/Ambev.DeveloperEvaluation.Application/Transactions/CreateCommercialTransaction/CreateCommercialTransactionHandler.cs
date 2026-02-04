using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;

/// <summary>
/// Handler for processing CreateCommercialTransactionCommand requests
/// </summary>
public class CreateCommercialTransactionHandler : IRequestHandler<CreateCommercialTransactionCommand, CreateCommercialTransactionResult>
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCommercialTransactionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateCommercialTransactionHandler
    /// </summary>
    /// <param name="transactionRepository">The transaction repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public CreateCommercialTransactionHandler(
        ICommercialTransactionRepository transactionRepository,
        IMapper mapper,
        ILogger<CreateCommercialTransactionHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CreateCommercialTransactionCommand request
    /// </summary>
    /// <param name="request">The CreateCommercialTransaction command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created transaction details</returns>
    public async Task<CreateCommercialTransactionResult> Handle(CreateCommercialTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating commercial transaction with code {TransactionCode}", request.TransactionCode);

        var validator = new CreateCommercialTransactionCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingTransaction = await _transactionRepository.GetByTransactionCodeAsync(request.TransactionCode, cancellationToken);
        if (existingTransaction is not null)
        {
            throw new InvalidOperationException($"Transaction with code {request.TransactionCode} already exists");
        }


        var businessPartner = new BusinessPartner
        {
            ExternalId = request.BusinessPartner.ExternalId,
            Name = request.BusinessPartner.Name,
            Email = request.BusinessPartner.Email,
            Document = request.BusinessPartner.Document
        };

        var operationalUnit = new OperationalUnit
        {
            ExternalId = request.OperationalUnit.ExternalId,
            Name = request.OperationalUnit.Name,
            Location = request.OperationalUnit.Location
        };

        var transaction = new CommercialTransaction
        {
            TransactionCode = request.TransactionCode,
            BusinessPartnerId = businessPartner.Id,
            BusinessPartner = businessPartner,
            OperationalUnitId = operationalUnit.Id,
            OperationalUnit = operationalUnit
        };


        foreach (var itemInfo in request.Items)
        {
            var product = new Product
            {
                ExternalId = itemInfo.Product.ExternalId,
                Name = itemInfo.Product.Name,
                Category = itemInfo.Product.Category,
                StandardPrice = itemInfo.Product.StandardPrice
            };

            transaction.AddItem(product.Id, itemInfo.Quantity, itemInfo.ItemPrice);

            var lastItem = transaction.Items.Last();
            lastItem.Product = product;
        }

        var createdTransaction = await _transactionRepository.CreateAsync(transaction, cancellationToken);

        var transactionCreatedEvent = new TransactionCreatedEvent(createdTransaction);
        _logger.LogInformation("CommercialTransactionCreated event: Transaction {TransactionId} with code {TransactionCode} created at {CreatedAt}",
            transactionCreatedEvent.Transaction.Id,
            transactionCreatedEvent.Transaction.TransactionCode,
            transactionCreatedEvent.OccurredAt);

        _logger.LogInformation("Commercial transaction created successfully with ID {TransactionId}", createdTransaction.Id);

        return _mapper.Map<CreateCommercialTransactionResult>(createdTransaction);
    }
}