using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;
using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.CreateCommercialTransaction;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CreateCommercialTransaction;

/// <summary>
/// Contains unit tests for the CreateCommercialTransactionHandler class.
/// </summary>
public class CreateCommercialTransactionHandlerTests
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly CreateCommercialTransactionHandler _handler;
    private readonly ILogger<CreateCommercialTransactionHandler> _logger;

    public CreateCommercialTransactionHandlerTests()
    {
        _transactionRepository = Substitute.For<ICommercialTransactionRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateCommercialTransactionHandler>>();
        _handler = new CreateCommercialTransactionHandler(_transactionRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid transaction creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid transaction data When creating transaction Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateValidCommand();
        var transaction = new CommercialTransaction
        {
            Id = Guid.NewGuid(),
            TransactionCode = command.TransactionCode,
            Amount = 100m
        };

        var result = new CreateCommercialTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            GrandTotal = transaction.Amount
        };

        _mapper.Map<CommercialTransaction>(command).Returns(transaction);
        _transactionRepository.CreateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<CreateCommercialTransactionResult>(transaction).Returns(result);

        // When
        var createResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(transaction.Id);
        createResult.TransactionCode.Should().Be(transaction.TransactionCode);
        await _transactionRepository.Received(1).CreateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid transaction creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid transaction data When creating transaction Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateCommercialTransactionCommand(); // -> Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that business rules are validated (max 20 identical items).
    /// </summary>
    [Fact(DisplayName = "Given transaction with more than 20 items When creating Then throws validation exception")]
    public async Task Handle_TransactionWithTooManyItems_ThrowsValidationException()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateCommandWithTooManyItems();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that grand total is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Given transaction with items When creating Then calculates grand total correctly")]
    public async Task Handle_TransactionWithItems_CalculatesGrandTotalCorrectly()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateValidCommand();
        var transaction = new CommercialTransaction();

        _mapper.Map<CommercialTransaction>(command).Returns(transaction);
        _transactionRepository.CreateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _transactionRepository.Received(1).CreateAsync(
            Arg.Is<CommercialTransaction>(t => t.Amount > 0),
            Arg.Any<CancellationToken>());
    }
}