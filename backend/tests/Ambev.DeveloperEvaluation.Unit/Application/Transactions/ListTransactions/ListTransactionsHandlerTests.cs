using Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.ListTransactions;

/// <summary>
/// Contains unit tests for the ListTransactionsHandler class.
/// </summary>
public class ListTransactionsHandlerTests
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListTransactionsHandler> _logger;
    private readonly ListTransactionsHandler _handler;

    public ListTransactionsHandlerTests()
    {
        _transactionRepository = Substitute.For<ICommercialTransactionRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<ListTransactionsHandler>>();
        _handler = new ListTransactionsHandler(_transactionRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid list request returns paginated transactions.
    /// </summary>
    [Fact(DisplayName = "Given valid list request When listing transactions Then returns paginated results")]
    public async Task Handle_ValidRequest_ReturnsPaginatedResults()
    {
        // Given
        var command = ListTransactionsTestData.GenerateValidCommand();
        var transactions = new List<CommercialTransaction>
        {
            CommercialTransactionTestData.GenerateValidTransaction(),
            CommercialTransactionTestData.GenerateValidTransaction()
        };

        var summaries = transactions.Select(t => new TransactionListItem
        {
            Id = t.Id,
            TransactionCode = t.TransactionCode,
            GrandTotal = t.Amount,
            TransactionDate = t.TransactionDate,
            BusinessPartnerName = t.BusinessPartner?.Name ?? "Test Partner",
            OperationalUnitName = t.OperationalUnit?.Name ?? "Test Unit",
            Status = t.Status.ToString(),
            ItemCount = t.Items.Count,
            CreatedAt = t.CreatedAt
        }).ToList();

        _transactionRepository.GetAllAsync(command.Page, command.Size, Arg.Any<CancellationToken>())
            .Returns(transactions);
        _mapper.Map<List<TransactionListItem>>(transactions).Returns(summaries);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Transactions.Should().NotBeNull();
        result.Transactions.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.TotalPages.Should().Be(1);
    }


    /// <summary>
    /// Tests that validation fails for invalid pagination parameters.
    /// </summary>
    [Fact(DisplayName = "Given invalid pagination When listing transactions Then throws validation exception")]
    public async Task Handle_InvalidPagination_ThrowsValidationException()
    {
        // Given
        var command = ListTransactionsTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that empty results return correct structure.
    /// </summary>
    [Fact(DisplayName = "Given no transactions When listing Then returns empty result")]
    public async Task Handle_NoTransactions_ReturnsEmptyResult()
    {
        // Given
        var command = ListTransactionsTestData.GenerateValidCommand();
        var emptyList = new List<CommercialTransaction>();
        var emptySummaries = new List<TransactionListItem>();

        _transactionRepository.GetAllAsync(command.Page, command.Size, Arg.Any<CancellationToken>())
            .Returns(emptyList);
        _mapper.Map<List<TransactionListItem>>(Arg.Is<List<CommercialTransaction>>(list =>
            list.Count == emptyList.Count))
        .Returns(emptySummaries);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Transactions.Should().NotBeNull();
        result.Transactions.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    /// <summary>
    /// Tests that the mapper is called with the correct transactions list.
    /// </summary>
    [Fact(DisplayName = "Given transactions list When mapping Then maps to transaction summaries")]
    public async Task Handle_ValidRequest_MapsTransactionsToSummaries()
    {
        // Given
        var command = ListTransactionsTestData.GenerateValidCommand();
        var transactions = new List<CommercialTransaction>
        {
            CommercialTransactionTestData.GenerateValidTransaction()
        };

        var summaries = new List<TransactionListItem>
        {
            new TransactionListItem
            {
                Id = transactions[0].Id,
                TransactionCode = transactions[0].TransactionCode,
                GrandTotal = transactions[0].Amount
            }
        };

        _transactionRepository.GetAllAsync(command.Page, command.Size, Arg.Any<CancellationToken>())
            .Returns(transactions);
        _mapper.Map<List<TransactionListItem>>(Arg.Is<List<CommercialTransaction>>(list =>
            list.Count == transactions.Count))
        .Returns(summaries);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Transactions.Should().NotBeNull();
        result.Transactions.Should().HaveCount(1);
        result.Transactions.First().Id.Should().Be(transactions[0].Id);
    }

    /// <summary>
    /// Tests pagination calculation.
    /// </summary>
    [Fact(DisplayName = "Given multiple transactions When listing with pagination Then calculates pages correctly")]
    public async Task Handle_MultipleTransactions_CalculatesPaginationCorrectly()
    {
        // Given
        var command = new ListTransactionsCommand(1, 2);

        var transactions = new List<CommercialTransaction>
        {
            CommercialTransactionTestData.GenerateValidTransaction(),
            CommercialTransactionTestData.GenerateValidTransaction(),
            CommercialTransactionTestData.GenerateValidTransaction()
        };

        var summaries = transactions.Select(t => new TransactionListItem
        {
            Id = t.Id,
            TransactionCode = t.TransactionCode,
            GrandTotal = t.Amount
        }).Take(2).ToList(); // Simulate pagination

        _transactionRepository.GetAllAsync(1, 2, Arg.Any<CancellationToken>())
            .Returns(transactions);
        _mapper.Map<List<TransactionListItem>>(Arg.Is<List<CommercialTransaction>>(list =>
            list.Count == transactions.Count))
        .Returns(summaries);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Transactions.Should().NotBeNull();
        result.TotalCount.Should().Be(3);
        result.TotalPages.Should().Be(2); // 3 items / 2 per page = 2 pages
        result.CurrentPage.Should().Be(1);
        result.PageSize.Should().Be(2);
    }
}