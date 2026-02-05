using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CommercialTransactionTests
{
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

    [Fact(DisplayName = "Grand total should be calculated correctly from items")]
    public void Given_TransactionWithItems_When_CalculatingTotal_Then_ShouldReturnCorrectSum()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateValidTransaction();
        var expectedTotal = transaction.Items.Where(i => !i.IsCancelled).Sum(i => i.ItemTotal);

        // Assert
        expectedTotal.Should().NotBe(0);
        transaction.Amount.Should().Be(expectedTotal);
    }

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

    [Fact(DisplayName = "Validation should fail for invalid transaction data")]
    public void Given_InvalidTransactionData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var transaction = new CommercialTransaction
        {
            TransactionCode = "",
            Items = new List<TransactionItem>()
        };

        // Act
        var result = transaction.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

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

    [Fact(DisplayName = "Amount should be recalculated when item is cancelled")]
    public void Given_TransactionWithItems_When_CancellingItem_Then_AmountShouldBeRecalculated()
    {
        // Arrange
        var transaction = CommercialTransactionTestData.GenerateTransactionForCalculationTest();
        var originalAmount = transaction.Amount;
        var itemToCancel = transaction.Items.First();
        var itemAmount = itemToCancel.ItemTotal;

        // Act
        transaction.CancelItem(itemToCancel.Id);

        // Assert
        var expectedAmount = originalAmount - itemAmount;
        Assert.Equal(expectedAmount, transaction.Amount);
    }

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

    [Fact(DisplayName = "Should apply 10% discount for 4-9 items")]
    public void Given_TransactionWith5Items_When_CalculatingDiscount_Then_ShouldApply10PercentDiscount()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateControlledTransactionItem(5, 100m);

        // Assert
        Assert.Equal(10m, item.DiscountPercentage);
        Assert.Equal(450m, item.ItemTotal);
    }

    [Fact(DisplayName = "Should apply 20% discount for 10-20 items")]
    public void Given_TransactionWith15Items_When_CalculatingDiscount_Then_ShouldApply20PercentDiscount()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateControlledTransactionItem(15, 100m);

        // Assert
        Assert.Equal(20m, item.DiscountPercentage);
        Assert.Equal(1200m, item.ItemTotal);
    }

    [Fact(DisplayName = "Should not apply discount for less than 4 items")]
    public void Given_TransactionWith3Items_When_CalculatingDiscount_Then_ShouldNotApplyDiscount()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateControlledTransactionItem(3, 100m);

        // Assert
        Assert.Equal(0m, item.DiscountPercentage);
        Assert.Equal(300m, item.ItemTotal);
    }
}