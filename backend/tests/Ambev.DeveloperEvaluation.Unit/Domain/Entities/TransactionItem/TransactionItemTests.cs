using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.Common;
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
        // Arrange - Use controlled data
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 6,
            ItemPrice = 50m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;

        // Act
        item.CalculateDiscount();
        item.CalculateItemTotal(); // Need to call this separately

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
        // Arrange - Use controlled data
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 12,
            ItemPrice = 25m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;

        // Act
        item.CalculateDiscount();
        item.CalculateItemTotal(); 

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
        // Arrange - Use controlled data
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 2,
            ItemPrice = 75m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;

        // Act
        item.CalculateDiscount();
        item.CalculateItemTotal(); 

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
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 2,
            ItemPrice = 50m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;

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
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 5,
            ItemPrice = 50m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;
        item.CalculateDiscount(); // Calculate discount for validation

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
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 0, // Invalid: zero
            ItemPrice = 10m,
            IsCancelled = false,
            DiscountPercentage = 0m
        };
        item.ProductId = item.Product.Id;

        // Act
        var result = item.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Detail.Contains("Quantity", StringComparison.OrdinalIgnoreCase));
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
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 25, // Invalid: above 20
            ItemPrice = 10m,
            IsCancelled = false,
            DiscountPercentage = 0m 
        };
        item.ProductId = item.Product.Id;

        // Act
        var result = item.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails for negative price.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for negative price")]
    public void Given_NegativePrice_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 5,
            ItemPrice = -10m, // Invalid: negative
            IsCancelled = false,
            DiscountPercentage = 10m 
        };
        item.ProductId = item.Product.Id;

        // Act
        var result = item.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("price", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests discount calculation for exactly 4 items (boundary test).
    /// </summary>
    [Fact(DisplayName = "Should calculate 10% discount for exactly 4 items")]
    public void Given_4Items_When_CalculatingDiscount_Then_ShouldApply10Percent()
    {
        // Arrange
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 4,
            ItemPrice = 100m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;

        // Act
        item.CalculateDiscount();
        item.CalculateItemTotal();

        // Assert
        Assert.Equal(10m, item.DiscountPercentage);
        Assert.Equal(360m, item.ItemTotal); // 4 * 100 * 0.9
    }

    /// <summary>
    /// Tests discount calculation for exactly 10 items (boundary test).
    /// </summary>
    [Fact(DisplayName = "Should calculate 20% discount for exactly 10 items")]
    public void Given_10Items_When_CalculatingDiscount_Then_ShouldApply20Percent()
    {
        // Arrange
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 10,
            ItemPrice = 100m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;

        // Act
        item.CalculateDiscount();
        item.CalculateItemTotal();

        // Assert
        Assert.Equal(20m, item.DiscountPercentage);
        Assert.Equal(800m, item.ItemTotal); // 10 * 100 * 0.8
    }

    /// <summary>
    /// Tests discount calculation for exactly 20 items (boundary test).
    /// </summary>
    [Fact(DisplayName = "Should calculate 20% discount for exactly 20 items")]
    public void Given_20Items_When_CalculatingDiscount_Then_ShouldApply20Percent()
    {
        // Arrange
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            Product = CommercialTransactionTestData.GenerateValidProduct(),
            Quantity = 20,
            ItemPrice = 100m,
            IsCancelled = false
        };
        item.ProductId = item.Product.Id;

        // Act
        item.CalculateDiscount();
        item.CalculateItemTotal();

        // Assert
        Assert.Equal(20m, item.DiscountPercentage);
        Assert.Equal(1600m, item.ItemTotal); // 20 * 100 * 0.8
    }

    /// <summary>
    /// Tests the Create factory method.
    /// </summary>
    [Fact(DisplayName = "Should create item with factory method")]
    public void Given_ValidParameters_When_UsingCreateMethod_Then_ShouldCreateItemWithCalculations()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 5;
        var itemPrice = 100m;

        // Act
        var item = TransactionItem.Create(productId, quantity, itemPrice);

        // Assert
        Assert.Equal(productId, item.ProductId);
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(itemPrice, item.ItemPrice);
        Assert.Equal(10m, item.DiscountPercentage); // 5 items = 10% discount
        Assert.Equal(450m, item.ItemTotal);
        Assert.False(item.IsCancelled);
    }

    /// <summary>
    /// Tests the UpdateQuantity method.
    /// </summary>
    [Fact(DisplayName = "Should update quantity and recalculate totals")]
    public void Given_ValidQuantity_When_UpdatingQuantity_Then_ShouldRecalculate()
    {
        // Arrange
        var item = TransactionItem.Create(Guid.NewGuid(), 2, 100m);

        // Act
        item.UpdateQuantity(8);

        // Assert
        Assert.Equal(8, item.Quantity);
        Assert.Equal(10m, item.DiscountPercentage); // 8 items = 10% discount
        Assert.Equal(720m, item.ItemTotal);
    }

    /// <summary>
    /// Tests that UpdateQuantity throws exception for invalid quantity.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when updating to invalid quantity")]
    public void Given_InvalidQuantity_When_UpdatingQuantity_Then_ShouldThrowException()
    {
        // Arrange
        var item = TransactionItem.Create(Guid.NewGuid(), 5, 100m);

        // Act & Assert
        Assert.Throws<DomainException>(() => item.UpdateQuantity(25));
        Assert.Throws<DomainException>(() => item.UpdateQuantity(0));
        Assert.Throws<DomainException>(() => item.UpdateQuantity(-1));
    }
}