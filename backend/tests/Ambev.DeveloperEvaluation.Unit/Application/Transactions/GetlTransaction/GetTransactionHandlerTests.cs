using Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.Common;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.GetTransaction;

/// <summary>
/// Contains unit tests for the GetTransactionHandler class.
/// </summary>
public class GetTransactionHandlerTests
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTransactionHandler> _logger;
    private readonly GetTransactionHandler _handler;

    public GetTransactionHandlerTests()
    {
        _transactionRepository = Substitute.For<ICommercialTransactionRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetTransactionHandler>>();
        _handler = new GetTransactionHandler(_transactionRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid get transaction request returns the transaction.
    /// </summary>
    [Fact(DisplayName = "Given valid transaction ID When getting transaction Then returns transaction")]
    public async Task Handle_ValidRequest_ReturnsTransaction()
    {
        // Given
        var command = GetTransactionHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.Id;

        var result = new GetTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            GrandTotal = transaction.Amount
        };

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<GetTransactionResult>(transaction).Returns(result);

        // When
        var getResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getResult.Should().NotBeNull();
        getResult.Id.Should().Be(transaction.Id);
        getResult.TransactionCode.Should().Be(transaction.TransactionCode);
        getResult.GrandTotal.Should().Be(transaction.Amount);
        await _transactionRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetTransactionResult>(transaction);
    }

    /// <summary>
    /// Tests that getting a non-existent transaction throws NotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent transaction ID When getting transaction Then throws NotFoundException")]
    public async Task Handle_NonExistentTransaction_ThrowsNotFoundException()
    {
        // Given
        var command = GetTransactionHandlerTestData.GenerateValidCommand();

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((CommercialTransaction?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that validation fails for empty ID.
    /// </summary>
    [Fact(DisplayName = "Given empty transaction ID When getting transaction Then throws validation exception")]
    public async Task Handle_EmptyId_ThrowsValidationException()
    {
        // Given
        var command = GetTransactionHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the mapper is called with the correct transaction.
    /// </summary>
    [Fact(DisplayName = "Given valid transaction When mapping Then maps transaction to result")]
    public async Task Handle_ValidRequest_MapsTransactionToResult()
    {
        // Given
        var command = GetTransactionHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.Id;

        var expectedResult = new GetTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            GrandTotal = transaction.Amount
        };

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<GetTransactionResult>(transaction).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeEquivalentTo(expectedResult);
        _mapper.Received(1).Map<GetTransactionResult>(Arg.Is<CommercialTransaction>(t =>
            t.Id == transaction.Id &&
            t.TransactionCode == transaction.TransactionCode &&
            t.Amount == transaction.Amount));
    }

    /// <summary>
    /// Tests that the result contains all expected properties.
    /// </summary>
    [Fact(DisplayName = "Given transaction with all properties When getting Then returns complete result")]
    public async Task Handle_TransactionWithAllProperties_ReturnsCompleteResult()
    {
        // Given
        var command = GetTransactionHandlerTestData.GenerateValidCommand();
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Id = command.Id;

        var result = new GetTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            GrandTotal = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            BusinessPartner = new BusinessPartnerResult
            {
               Name = transaction.BusinessPartner?.Name ?? "Test Partner",
            },
            OperationalUnit = new OperationalUnitResult
            {
                Name = transaction.OperationalUnit?.Name ?? "Test Unit"
            }
        };

        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<GetTransactionResult>(transaction).Returns(result);

        // When
        var getResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getResult.Should().NotBeNull();
        getResult.Id.Should().Be(transaction.Id);
        getResult.TransactionCode.Should().Be(transaction.TransactionCode);
        getResult.GrandTotal.Should().Be(transaction.Amount);
        getResult.TransactionDate.Should().Be(transaction.TransactionDate);
        getResult.BusinessPartner.Name.Should().NotBeNullOrEmpty();
        getResult.OperationalUnit.Name.Should().NotBeNullOrEmpty();
    }
}