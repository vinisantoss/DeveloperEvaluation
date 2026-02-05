using Ambev.DeveloperEvaluation.Domain.Transaction.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.Common;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the CommercialTransactionValidator class.
/// Tests cover transaction validation scenarios including business rules.
/// </summary>
public class CommercialTransactionValidatorTests
{
    private readonly CommercialTransactionValidator _validator;

    public CommercialTransactionValidatorTests()
    {
        _validator = new CommercialTransactionValidator();
    }

    /// <summary>
    /// Tests that validation passes for valid transaction data.
    /// </summary>
    [Fact(DisplayName = "Valid transaction should pass validation")]
    public void Given_ValidTransaction_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when transaction code is empty.
    /// </summary>
    [Fact(DisplayName = "Empty transaction code should fail validation")]
    public void Given_EmptyTransactionCode_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.TransactionCode = string.Empty;

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransactionCode)
            .WithErrorMessage("Transaction code is required");
    }

    /// <summary>
    /// Tests that validation fails when transaction code exceeds maximum length.
    /// </summary>
    [Fact(DisplayName = "Transaction code exceeding maximum length should fail validation")]
    public void Given_TransactionCodeExceeding50Characters_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.TransactionCode = new string('A', 51); // 51 characters

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransactionCode)
            .WithErrorMessage("Transaction code cannot exceed 50 characters");
    }

    /// <summary>
    /// Tests that validation fails when business partner ID is empty.
    /// </summary>
    [Fact(DisplayName = "Empty business partner ID should fail validation")]
    public void Given_EmptyBusinessPartnerId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.BusinessPartnerId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BusinessPartnerId)
            .WithErrorMessage("Business partner is required");
    }

    /// <summary>
    /// Tests that validation fails when operational unit ID is empty.
    /// </summary>
    [Fact(DisplayName = "Empty operational unit ID should fail validation")]
    public void Given_EmptyOperationalUnitId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.OperationalUnitId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OperationalUnitId)
            .WithErrorMessage("Operational unit is required");
    }

    /// <summary>
    /// Tests that validation fails when transaction date is empty.
    /// </summary>
    [Fact(DisplayName = "Empty transaction date should fail validation")]
    public void Given_EmptyTransactionDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.TransactionDate = default(DateTime);

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransactionDate)
            .WithErrorMessage("Transaction date is required");
    }

    /// <summary>
    /// Tests that validation fails when transaction date is in the future.
    /// </summary>
    [Fact(DisplayName = "Future transaction date should fail validation")]
    public void Given_FutureTransactionDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.TransactionDate = DateTime.UtcNow.AddDays(1);

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TransactionDate)
            .WithErrorMessage("Transaction date cannot be in the future");
    }

    /// <summary>
    /// Tests that validation fails when transaction has no items.
    /// </summary>
    [Fact(DisplayName = "Transaction without items should fail validation")]
    public void Given_TransactionWithoutItems_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Items = new List<TransactionItem>();

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items)
            .WithErrorMessage("Transaction must have at least one item");
    }

    /// <summary>
    /// Tests that validation fails when transaction has invalid items.
    /// </summary>
    [Fact(DisplayName = "Transaction with invalid items should fail validation")]
    public void Given_TransactionWithInvalidItems_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Items.First().Quantity = 0; // Invalid quantity

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
    }

    /// <summary>
    /// Tests various valid transaction code lengths.
    /// </summary>
    [Theory(DisplayName = "Valid transaction code lengths should pass validation")]
    [InlineData("A")]
    [InlineData("ABC123")]
    [InlineData("TRANSACTION_CODE_12345")]
    public void Given_ValidTransactionCodeLength_When_Validated_Then_ShouldNotHaveErrors(string transactionCode)
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.TransactionCode = transactionCode;

        // Act
        var result = _validator.TestValidate(transaction);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.TransactionCode);
    }
}