using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the TransactionItem entity class.
/// Tests cover discount calculations and business rules.
/// </summary>
public class TransactionItemTests
{
    /// <summary>
    /// Tests that discount is calculated correctly for 4-9 items.
    /// </summary>
    [Fact(DisplayName = "Should calculate 10% discount for 4-9 items")]
    public void Given_6Items_When_CalculatingDiscount_Then_ShouldApply10Percent()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = 6;
        item.ItemPrice = 50m;

        // Act
        item.CalculateDiscount();

        // Assert
        Assert.Equal(10m, item.DiscountPercentage);
        Assert.Equal(270m, item.ItemTotal); // 6 * 50 * 0.9
    }

    /// <summary>
    /// Tests that discount is calculated correctly for 10-20 items.
    /// </summary>
    [Fact(DisplayName = "Should calculate 20% discount for 10-20 items")]
    public void Given_12Items_When_CalculatingDiscount_Then_ShouldApply20Percent()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = 12;
        item.ItemPrice = 25m;

        // Act
        item.CalculateDiscount();

        // Assert
        Assert.Equal(20m, item.DiscountPercentage);
        Assert.Equal(240m, item.ItemTotal); // 12 * 25 * 0.8
    }

    /// <summary>
    /// Tests that no discount is applied for less than 4 items.
    /// </summary>
    [Fact(DisplayName = "Should not apply discount for less than 4 items")]
    public void Given_2Items_When_CalculatingDiscount_Then_ShouldNotApplyDiscount()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = 2;
        item.ItemPrice = 75m;

        // Act
        item.CalculateDiscount();

        // Assert
        Assert.Equal(0m, item.DiscountPercentage);
        Assert.Equal(150m, item.ItemTotal); // 2 * 75
    }

    /// <summary>
    /// Tests that item can be cancelled.
    /// </summary>
    [Fact(DisplayName = "Item should be cancelled when Cancel is called")]
    public void Given_ActiveItem_When_Cancelled_Then_ShouldBeCancelled()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.IsCancelled = false;

        // Act
        item.Cancel();

        // Assert
        Assert.True(item.IsCancelled);
    }

    /// <summary>
    /// Tests that validation passes for valid item data.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid item data")]
    public void Given_ValidItemData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();

        // Act
        var result = item.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails for invalid quantity.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid quantity")]
    public void Given_InvalidQuantity_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var item = new TransactionItem
        {
            Quantity = 0, // Invalid: zero
            ItemPrice = 10m
        };

        // Act
        var result = item.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails for quantity above 20.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for quantity above 20")]
    public void Given_QuantityAbove20_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var item = new TransactionItem
        {
            Quantity = 25, // Invalid: above 20
            ItemPrice = 10m
        };

        // Act
        var result = item.Validate();

        // Assert
        Assert.False(result.IsValid);
    }
}