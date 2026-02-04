using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.CancelTransaction;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CancelTransactionItem;

/// <summary>
/// Contains unit tests for the CancelTransactionItemHandler class.
/// </summary>
public class CancelTransactionItemHandlerTests
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly CancelTransactionItemHandler _handler;
    private readonly ILogger<CancelTransactionItemHandler> _logger;

    public CancelTransactionItemHandlerTests()
    {
        _transactionRepository = Substitute.For<ICommercialTransactionRepository>();
        _logger = _logger = Substitute.For<ILogger<CancelTransactionItemHandler>>();
        _handler = new CancelTransactionItemHandler(_transactionRepository, _logger);
    }

    /// <summary>
    /// Tests that a valid cancel item request cancels the item.
    /// </summary>
    [Fact(DisplayName = "Given valid transaction and item IDs When cancelling item Then item is cancelled")]
    public async Task Handle_ValidRequest_CancelsItem()
    {
        // Given
        var command = CancelTransactionItemHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.TransactionId;

        var itemToCancel = transaction.Items.First();
        itemToCancel.Id = command.ItemId;
        var originalGrandTotal = transaction.Amount;

        _transactionRepository.GetByIdAsync(command.TransactionId, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _transactionRepository.UpdateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        itemToCancel.IsCancelled.Should().BeTrue();
        transaction.Amount.Should().BeLessThan(originalGrandTotal);
        await _transactionRepository.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that cancelling a non-existent transaction throws NotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent transaction When cancelling item Then throws NotFoundException")]
    public async Task Handle_NonExistentTransaction_ThrowsNotFoundException()
    {
        // Given
        var command = CancelTransactionItemHandlerTestData.GenerateValidCommand();

        _transactionRepository.GetByIdAsync(command.TransactionId, Arg.Any<CancellationToken>())
            .Returns((CommercialTransaction?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that cancelling a non-existent item throws NotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent item When cancelling Then throws NotFoundException")]
    public async Task Handle_NonExistentItem_ThrowsNotFoundException()
    {
        // Given
        var command = CancelTransactionItemHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Items = new List<TransactionItem>();

        _transactionRepository.GetByIdAsync(command.TransactionId, Arg.Any<CancellationToken>())
            .Returns(transaction);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that cancelling an already cancelled item throws InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given already cancelled item When cancelling Then throws InvalidOperationException")]
    public async Task Handle_AlreadyCancelledItem_ThrowsInvalidOperationException()
    {
        // Given
        var command = CancelTransactionItemHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.TransactionId;

        var itemToCancel = transaction.Items.First();
        itemToCancel.Id = command.ItemId;
        itemToCancel.IsCancelled = true;

        _transactionRepository.GetByIdAsync(command.TransactionId, Arg.Any<CancellationToken>())
            .Returns(transaction);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*already cancelled*");
    }

    /// <summary>
    /// Tests that validation fails for empty IDs.
    /// </summary>
    [Fact(DisplayName = "Given empty IDs When cancelling item Then throws validation exception")]
    public async Task Handle_EmptyIds_ThrowsValidationException()
    {
       
        // Given
        var command = CancelTransactionItemHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateCancelledTransaction();
        command.TransactionId = Guid.Empty;

        _transactionRepository.GetByIdAsync(command.TransactionId, Arg.Any<CancellationToken>())
            .Returns(transaction);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that grand total is recalculated when item is cancelled.
    /// </summary>
    [Fact(DisplayName = "Given item cancellation When processing Then recalculates grand total")]
    public async Task Handle_ItemCancellation_RecalculatesGrandTotal()
    {
        // Given
        var command = CancelTransactionItemHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.TransactionId;

        var itemToCancel = transaction.Items.First();
        itemToCancel.Id = command.ItemId;
        var itemTotal = itemToCancel.ItemTotal;
        var originalGrandTotal = transaction.Amount;

        _transactionRepository.GetByIdAsync(command.TransactionId, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _transactionRepository.UpdateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        transaction.Amount.Should().Be(originalGrandTotal - itemTotal);
    }
}