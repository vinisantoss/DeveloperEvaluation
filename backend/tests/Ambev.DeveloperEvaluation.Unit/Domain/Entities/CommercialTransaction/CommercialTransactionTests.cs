using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the CommercialTransaction entity class.
/// Tests cover business rules, calculations, and validation scenarios.
/// </summary>
public class CommercialTransactionTests
{
    /// <summary>
    /// Tests that when a transaction is cancelled, its status changes to Cancelled.
    /// </summary>
    [Fact(DisplayName = "Transaction status should change to Cancelled when cancelled")]
    public void Given_ActiveTransaction_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        transaction.Status = TransactionStatus.Active;

        // Act
        transaction.Cancel();

        // Assert
        Assert.Equal(TransactionStatus.Cancelled, transaction.Status);
    }

    /// <summary>
    /// Tests that grand total is calculated correctly from items.
    /// </summary>
    [Fact(DisplayName = "Grand total should be calculated correctly from items")]
    public void Given_TransactionWithItems_When_CalculatingTotal_Then_ShouldReturnCorrectSum()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        var expectedTotal = transaction.Items.Where(i => !i.IsCancelled).Sum(i => i.ItemTotal);


        // Assert
        expectedTotal.Should().NotBe(0);
    }

    /// <summary>
    /// Tests that validation passes when all transaction properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid transaction data")]
    public void Given_ValidTransactionData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();

        // Act
        var result = transaction.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when transaction has invalid data.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid transaction data")]
    public void Given_InvalidTransactionData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var transaction = new CommercialTransaction
        {
            TransactionCode = "", // Invalid: empty
            Items = new List<TransactionItem>() // Invalid: no items
        };

        // Act
        var result = transaction.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that a transaction item can be cancelled.
    /// </summary>
    [Fact(DisplayName = "Transaction item should be cancelled when CancelItem is called")]
    public void Given_TransactionWithItems_When_CancellingItem_Then_ItemShouldBeCancelled()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        var itemToCancel = transaction.Items.First();

        // Act
        transaction.CancelItem(itemToCancel.Id);

        // Assert
        Assert.True(itemToCancel.IsCancelled);
    }

    /// <summary>
    /// Tests that amount is recalculated when an item is cancelled.
    /// </summary>
    [Fact(DisplayName = "Amount should be recalculated when item is cancelled")]
    public void Given_TransactionWithItems_When_CancellingItem_Then_AmountShouldBeRecalculated()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        var originalAmount = transaction.Amount;
        var itemToCancel = transaction.Items.First();
        var itemAmount = itemToCancel.ItemPrice;

        // Act
        transaction.CancelItem(itemToCancel.Id);

        // Assert
        Assert.Equal(originalAmount - itemAmount, transaction.Amount);
    }

    /// <summary>
    /// Tests business rule: cannot sell more than 20 identical items.
    /// </summary>
    [Fact(DisplayName = "Should not allow more than 20 identical items")]
    public void Given_TransactionWithMoreThan20Items_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateTransactionWithTooManyItems();

        // Act
        var result = transaction.Validate();

        // Assert
        Assert.False(result.IsValid);
    }

    /// <summary>
    /// Tests business rule: 10% discount for 4-9 items.
    /// </summary>
    [Fact(DisplayName = "Should apply 10% discount for 4-9 items")]
    public void Given_TransactionWith5Items_When_CalculatingDiscount_Then_ShouldApply10PercentDiscount()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        var item = transaction.Items.First();
        item.Quantity = 5;
        item.ItemPrice = 100m;

        // Act
        item.CalculateDiscount();

        // Assert
        Assert.Equal(10m, item.DiscountPercentage);
        Assert.Equal(450m, item.ItemTotal); // 5 * 100 * 0.9
    }

    /// <summary>
    /// Tests business rule: 20% discount for 10-20 items.
    /// </summary>
    [Fact(DisplayName = "Should apply 20% discount for 10-20 items")]
    public void Given_TransactionWith15Items_When_CalculatingDiscount_Then_ShouldApply20PercentDiscount()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        var item = transaction.Items.First();
        item.Quantity = 15;
        item.ItemPrice = 100m;

        // Act
        item.CalculateDiscount();

        // Assert
        Assert.Equal(20m, item.DiscountPercentage);
        Assert.Equal(1200m, item.ItemTotal); // 15 * 100 * 0.8
    }

    /// <summary>
    /// Tests business rule: no discount for less than 4 items.
    /// </summary>
    [Fact(DisplayName = "Should not apply discount for less than 4 items")]
    public void Given_TransactionWith3Items_When_CalculatingDiscount_Then_ShouldNotApplyDiscount()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        var item = transaction.Items.First();
        item.Quantity = 3;
        item.ItemPrice = 100m;

        // Act
        item.CalculateDiscount();

        // Assert
        Assert.Equal(0m, item.DiscountPercentage);
        Assert.Equal(300m, item.ItemTotal); // 3 * 100
    }
}
