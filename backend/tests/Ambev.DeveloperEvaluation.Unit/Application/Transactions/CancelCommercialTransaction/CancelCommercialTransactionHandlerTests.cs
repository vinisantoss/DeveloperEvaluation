using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.Common;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CancelTransaction;

/// <summary>
/// Contains unit tests for the CancelTransactionHandler class.
/// </summary>
public class CancelTransactionHandlerTests
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelTransactionHandler> _logger;
    private readonly CancelTransactionHandler _handler;

    public CancelTransactionHandlerTests()
    {
        _transactionRepository = Substitute.For<ICommercialTransactionRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelTransactionHandler>>();
        _handler = new CancelTransactionHandler(_transactionRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid cancel request cancels the transaction.
    /// </summary>
    [Fact(DisplayName = "Given valid transaction ID When cancelling transaction Then transaction is cancelled")]
    public async Task Handle_ValidRequest_CancelsTransaction()
    {
        // Given
        var command = CancelTransactionTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.Id;
        transaction.Status = TransactionStatus.Active;

        var result = new CancelTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            Message = $"Transaction {transaction.TransactionCode} has been successfully cancelled"
        };

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _transactionRepository.UpdateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<CancelTransactionResult>(transaction).Returns(result);

        // When
        var cancelResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelResult.Should().NotBeNull();
        cancelResult.Id.Should().Be(transaction.Id);
        transaction.Status.Should().Be(TransactionStatus.Cancelled);
        await _transactionRepository.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CancelTransactionResult>(transaction);
    }

    /// <summary>
    /// Tests that cancelling a non-existent transaction throws NotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent transaction ID When cancelling Then throws NotFoundException")]
    public async Task Handle_NonExistentTransaction_ThrowsNotFoundException()
    {
        // Given
        var command = CancelTransactionTestData.GenerateValidCommand();

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((CommercialTransaction?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that cancelling an already cancelled transaction throws InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given already cancelled transaction When cancelling Then throws InvalidOperationException")]
    public async Task Handle_AlreadyCancelledTransaction_ThrowsInvalidOperationException()
    {
        // Given
        var command = CancelTransactionTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateCancelledTransaction();
        command.Id = Guid.Empty;

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that validation fails for empty ID.
    /// </summary>
    [Fact(DisplayName = "Given empty transaction ID When cancelling Then throws validation exception")]
    public async Task Handle_EmptyId_ThrowsValidationException()
    {
        // Given
        var command = CancelTransactionTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the mapper is called to create the result.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps transaction to result")]
    public async Task Handle_ValidRequest_MapsTransactionToResult()
    {
        // Given
        var command = CancelTransactionTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.Id;
        transaction.Status = TransactionStatus.Active;

        var result = new CancelTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
        };

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _transactionRepository.UpdateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<CancelTransactionResult>(transaction).Returns(result);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<CancelTransactionResult>(Arg.Is<CommercialTransaction>(t =>
            t.Id == transaction.Id &&
            t.Status == TransactionStatus.Cancelled &&
            t.TransactionCode == transaction.TransactionCode));
    }
}