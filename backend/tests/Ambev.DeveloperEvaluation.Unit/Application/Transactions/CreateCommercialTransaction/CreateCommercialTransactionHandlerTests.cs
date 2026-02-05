using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
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
    private readonly IProductRepository _productRepository;
    private readonly IBusinessPartnerRepository _businessPartnerRepository;
    private readonly IOperationalUnitRepository _operationalUnitRepository;
    private readonly IMapper _mapper;
    private readonly CreateCommercialTransactionHandler _handler;
    private readonly ILogger<CreateCommercialTransactionHandler> _logger;

    public CreateCommercialTransactionHandlerTests()
    {
        _transactionRepository = Substitute.For<ICommercialTransactionRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _businessPartnerRepository = Substitute.For<IBusinessPartnerRepository>();
        _operationalUnitRepository = Substitute.For<IOperationalUnitRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateCommercialTransactionHandler>>();
        _handler = new CreateCommercialTransactionHandler(_transactionRepository, _productRepository, _businessPartnerRepository, _operationalUnitRepository, _mapper, _logger);
    }

    /// <summary>
    /// Sets up valid repository mocks for successful test scenarios
    /// </summary>
    private void SetupValidRepositoryMocks(CreateCommercialTransactionCommand command)
    {
        // Setup BusinessPartner mock
        var businessPartner = new BusinessPartner
        {
            Id = Guid.NewGuid(),
            ExternalId = command.BusinessPartner.ExternalId,
            Name = command.BusinessPartner.Name,
            Email = command.BusinessPartner.Email,
            Document = command.BusinessPartner.Document
        };

        _businessPartnerRepository.GetByExternalIdAsync(command.BusinessPartner.ExternalId, Arg.Any<CancellationToken>())
            .Returns(businessPartner);

        // Setup OperationalUnit mock
        var operationalUnit = new OperationalUnit
        {
            Id = Guid.NewGuid(),
            ExternalId = command.OperationalUnit.ExternalId,
            Name = command.OperationalUnit.Name,
            Location = command.OperationalUnit.Location
        };

        _operationalUnitRepository.GetByExternalIdAsync(command.OperationalUnit.ExternalId, Arg.Any<CancellationToken>())
            .Returns(operationalUnit);

        // Setup Product mocks for each item
        foreach (var item in command.Items)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                ExternalId = item.Product.ExternalId,
                Name = item.Product.Name,
                Category = item.Product.Category,
                StandardPrice = item.Product.StandardPrice
            };

            _productRepository.GetByExternalIdAsync(item.Product.ExternalId, Arg.Any<CancellationToken>())
                .Returns(product);
        }

        // Setup no existing transaction
        _transactionRepository.GetByTransactionCodeAsync(command.TransactionCode, Arg.Any<CancellationToken>())
            .Returns((CommercialTransaction)null);
    }

    /// <summary>
    /// Tests that a valid transaction creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid transaction data When creating transaction Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateSimpleValidCommand();
        SetupValidRepositoryMocks(command);

        var transaction = new CommercialTransaction
        {
            Id = Guid.NewGuid(),
            TransactionCode = command.TransactionCode,
            Amount = 10.00m
        };

        var result = new CreateCommercialTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            GrandTotal = transaction.Amount
        };

        _transactionRepository.CreateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<CreateCommercialTransactionResult>(Arg.Any<CommercialTransaction>()).Returns(result);

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
        var command = new CreateCommercialTransactionCommand();

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
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("*Cannot sell more than 20 identical items*");
    }

    /// <summary>
    /// Tests that grand total is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Given transaction with items When creating Then calculates grand total correctly")]
    public async Task Handle_TransactionWithItems_CalculatesGrandTotalCorrectly()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateCommandForGrandTotalTest();
        var expectedGrandTotal = 110.00m;
        SetupValidRepositoryMocks(command);

        var transaction = new CommercialTransaction
        {
            Id = Guid.NewGuid(),
            TransactionCode = command.TransactionCode,
            Amount = expectedGrandTotal
        };

        var result = new CreateCommercialTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            GrandTotal = expectedGrandTotal
        };

        _transactionRepository.CreateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<CreateCommercialTransactionResult>(Arg.Any<CommercialTransaction>()).Returns(result);

        // When
        var createResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createResult.Should().NotBeNull();
        createResult.GrandTotal.Should().Be(expectedGrandTotal);
        await _transactionRepository.Received(1).CreateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that transaction with exactly 20 items is valid.
    /// </summary>
    [Fact(DisplayName = "Given transaction with exactly 20 items When creating Then should succeed")]
    public async Task Handle_TransactionWithExactly20Items_ShouldSucceed()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateCommandWithExactly20Items();
        SetupValidRepositoryMocks(command);

        var transaction = new CommercialTransaction
        {
            Id = Guid.NewGuid(),
            TransactionCode = command.TransactionCode,
            Amount = 200m
        };

        var result = new CreateCommercialTransactionResult
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            GrandTotal = 200m
        };

        _transactionRepository.CreateAsync(Arg.Any<CommercialTransaction>(), Arg.Any<CancellationToken>())
            .Returns(transaction);
        _mapper.Map<CreateCommercialTransactionResult>(Arg.Any<CommercialTransaction>()).Returns(result);

        // When
        var createResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createResult.Should().NotBeNull();
        createResult.GrandTotal.Should().Be(200m);
    }

    /// <summary>
    /// Tests that non-existent business partner throws domain exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent business partner When creating transaction Then throws domain exception")]
    public async Task Handle_NonExistentBusinessPartner_ThrowsDomainException()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateSimpleValidCommand();

        _businessPartnerRepository.GetByExternalIdAsync(command.BusinessPartner.ExternalId, Arg.Any<CancellationToken>())
            .Returns((BusinessPartner)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Business Partner with ExternalId '{command.BusinessPartner.ExternalId}' does not exists.");
    }

    /// <summary>
    /// Tests that non-existent operational unit throws domain exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent operational unit When creating transaction Then throws domain exception")]
    public async Task Handle_NonExistentOperationalUnit_ThrowsDomainException()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateSimpleValidCommand();

        var businessPartner = new BusinessPartner
        {
            Id = Guid.NewGuid(),
            ExternalId = command.BusinessPartner.ExternalId,
            Name = command.BusinessPartner.Name,
            Email = command.BusinessPartner.Email,
            Document = command.BusinessPartner.Document
        };

        _businessPartnerRepository.GetByExternalIdAsync(command.BusinessPartner.ExternalId, Arg.Any<CancellationToken>())
            .Returns(businessPartner);

        _operationalUnitRepository.GetByExternalIdAsync(command.OperationalUnit.ExternalId, Arg.Any<CancellationToken>())
            .Returns((OperationalUnit)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Operational Unit with ExternalId '{command.OperationalUnit.ExternalId}' does not exists.");
    }

    /// <summary>
    /// Tests that non-existent product throws domain exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent product When creating transaction Then throws domain exception")]
    public async Task Handle_NonExistentProduct_ThrowsDomainException()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateSimpleValidCommand();

        var businessPartner = new BusinessPartner
        {
            Id = Guid.NewGuid(),
            ExternalId = command.BusinessPartner.ExternalId,
            Name = command.BusinessPartner.Name,
            Email = command.BusinessPartner.Email,
            Document = command.BusinessPartner.Document
        };

        var operationalUnit = new OperationalUnit
        {
            Id = Guid.NewGuid(),
            ExternalId = command.OperationalUnit.ExternalId,
            Name = command.OperationalUnit.Name,
            Location = command.OperationalUnit.Location
        };

        _businessPartnerRepository.GetByExternalIdAsync(command.BusinessPartner.ExternalId, Arg.Any<CancellationToken>())
            .Returns(businessPartner);

        _operationalUnitRepository.GetByExternalIdAsync(command.OperationalUnit.ExternalId, Arg.Any<CancellationToken>())
            .Returns(operationalUnit);

        _productRepository.GetByExternalIdAsync(command.Items.First().Product.ExternalId, Arg.Any<CancellationToken>())
            .Returns((Product)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Produto com ExternalId '{command.Items.First().Product.ExternalId}' não encontrado.");
    }

    /// <summary>
    /// Tests that duplicate transaction code throws invalid operation exception.
    /// </summary>
    [Fact(DisplayName = "Given duplicate transaction code When creating transaction Then throws invalid operation exception")]
    public async Task Handle_DuplicateTransactionCode_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateCommercialTransactionHandlerTestData.GenerateSimpleValidCommand();

        var existingTransaction = new CommercialTransaction
        {
            Id = Guid.NewGuid(),
            TransactionCode = command.TransactionCode
        };

        _transactionRepository.GetByTransactionCodeAsync(command.TransactionCode, Arg.Any<CancellationToken>())
            .Returns(existingTransaction);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Transaction with code {command.TransactionCode} already exists");
    }
}