using Ambev.DeveloperEvaluation.Domain.Transaction.Validation;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.Common;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the TransactionItemValidator class.
/// Tests cover item validation scenarios including business rules for discounts.
/// </summary>
public class TransactionItemValidatorTests
{
    private readonly TransactionItemValidator _validator;

    public TransactionItemValidatorTests()
    {
        _validator = new TransactionItemValidator();
    }

    /// <summary>
    /// Tests that validation passes for valid transaction item data.
    /// </summary>
    [Fact(DisplayName = "Valid transaction item should pass validation")]
    public void Given_ValidTransactionItem_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = 5;
        item.DiscountPercentage = 10; // Valid for 4+ items

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when product ID is empty.
    /// </summary>
    [Fact(DisplayName = "Empty product ID should fail validation")]
    public void Given_EmptyProductId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.ProductId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorMessage("Product is required");
    }

    /// <summary>
    /// Tests that validation fails when quantity is zero or negative.
    /// </summary>
    [Theory(DisplayName = "Zero or negative quantity should fail validation")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Given_InvalidQuantity_When_Validated_Then_ShouldHaveError(int quantity)
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = quantity;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
            .WithErrorMessage("Quantity must be greater than zero");
    }

    /// <summary>
    /// Tests that validation fails when quantity exceeds 20.
    /// </summary>
    [Fact(DisplayName = "Quantity exceeding 20 should fail validation")]
    public void Given_QuantityExceeding20_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = 21;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
            .WithErrorMessage("Cannot sell more than 20 identical items");
    }

    /// <summary>
    /// Tests that validation fails when item price is zero or negative.
    /// </summary>
    [Theory(DisplayName = "Zero or negative item price should fail validation")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.50)]
    public void Given_InvalidItemPrice_When_Validated_Then_ShouldHaveError(decimal price)
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.ItemPrice = price;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ItemPrice)
            .WithErrorMessage("Item price must be greater than zero");
    }

    /// <summary>
    /// Tests that validation fails when discount percentage is negative.
    /// </summary>
    [Fact(DisplayName = "Negative discount percentage should fail validation")]
    public void Given_NegativeDiscountPercentage_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.DiscountPercentage = -5;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DiscountPercentage)
            .WithErrorMessage("Discount cannot be negative");
    }

    /// <summary>
    /// Tests that validation fails when discount percentage exceeds 100.
    /// </summary>
    [Fact(DisplayName = "Discount percentage exceeding 100 should fail validation")]
    public void Given_DiscountPercentageExceeding100_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.DiscountPercentage = 150;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DiscountPercentage)
            .WithErrorMessage("Discount cannot exceed 100%");
    }

    /// <summary>
    /// Tests business rule: purchases below 4 items cannot have discount.
    /// </summary>
    [Theory(DisplayName = "Items below 4 quantity with discount should fail validation")]
    [InlineData(1, 10)]
    [InlineData(2, 5)]
    [InlineData(3, 20)]
    public void Given_ItemsBelowFourWithDiscount_When_Validated_Then_ShouldHaveError(int quantity, decimal discount)
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = quantity;
        item.DiscountPercentage = discount;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Discount rules violation: purchases below 4 items cannot have discount, 4+ items get 10%, 10-20 items get 20%");
    }

    /// <summary>
    /// Tests business rule: 4-9 items should have 10% discount.
    /// </summary>
    [Theory(DisplayName = "Items 4-9 quantity with wrong discount should fail validation")]
    [InlineData(4, 0)]   // Should have 10% discount
    [InlineData(5, 20)]  // Should have 10% discount, not 20%
    [InlineData(9, 5)]   // Should have 10% discount, not 5%
    public void Given_Items4To9WithWrongDiscount_When_Validated_Then_ShouldHaveError(int quantity, decimal discount)
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = quantity;
        item.DiscountPercentage = discount;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Discount rules violation: purchases below 4 items cannot have discount, 4+ items get 10%, 10-20 items get 20%");
    }

    /// <summary>
    /// Tests business rule: 10-20 items should have 20% discount.
    /// </summary>
    [Theory(DisplayName = "Items 10-20 quantity with wrong discount should fail validation")]
    [InlineData(10, 10)]  // Should have 20% discount, not 10%
    [InlineData(15, 0)]   // Should have 20% discount
    [InlineData(20, 15)]  // Should have 20% discount, not 15%
    public void Given_Items10To20WithWrongDiscount_When_Validated_Then_ShouldHaveError(int quantity, decimal discount)
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = quantity;
        item.DiscountPercentage = discount;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Discount rules violation: purchases below 4 items cannot have discount, 4+ items get 10%, 10-20 items get 20%");
    }

    /// <summary>
    /// Tests business rule: valid discount scenarios should pass.
    /// </summary>
    [Theory(DisplayName = "Valid discount scenarios should pass validation")]
    [InlineData(1, 0)]   // Below 4 items, no discount
    [InlineData(3, 0)]   // Below 4 items, no discount
    [InlineData(4, 10)]  // 4+ items, 10% discount
    [InlineData(9, 10)]  // 4-9 items, 10% discount
    [InlineData(10, 20)] // 10+ items, 20% discount
    [InlineData(20, 20)] // 10-20 items, 20% discount
    public void Given_ValidDiscountScenarios_When_Validated_Then_ShouldNotHaveErrors(int quantity, decimal discount)
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = quantity;
        item.DiscountPercentage = discount;

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    /// <summary>
    /// Tests valid quantity range.
    /// </summary>
    [Theory(DisplayName = "Valid quantity range should pass validation")]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void Given_ValidQuantity_When_Validated_Then_ShouldNotHaveErrors(int quantity)
    {
        // Arrange
        var item = CommercialTransactionTestData.GenerateValidTransactionItem();
        item.Quantity = quantity;
        item.DiscountPercentage = quantity switch
        {
            < 4 => 0,
            >= 4 and < 10 => 10,
            >= 10 and <= 20 => 20,
            _ => 0
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Quantity);
    }
}