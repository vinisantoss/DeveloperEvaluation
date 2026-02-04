using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Product entity class.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Tests that validation passes when all product properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid product data")]
    public async Task Given_ValidProductData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();

        // Act
        var result = await product.ValidateAsync();

        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    /// Tests that validation fails when product properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid product data")]
    public async Task Given_InvalidProductData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var product = new Product
        {
            ExternalId = "", // Invalid: empty
            Name = "", // Invalid: empty
            Category = "", // Invalid: empty
            StandardPrice = -1m // Invalid: negative
        };

        // Act
        var result = await product.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
    }

    /// <summary>
    /// Tests that standard price validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate standard price is positive")]
    public async Task Given_NegativePrice_When_Validated_Then_ShouldHavePriceError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.StandardPrice = -10m;

        // Act
        var result = await product.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, e => e.Detail.Contains("price", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that required fields validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate required fields")]
    public async Task Given_EmptyRequiredFields_When_Validated_Then_ShouldHaveRequiredFieldErrors()
    {
        // Arrange
        var product = new Product
        {
            ExternalId = "",
            Name = "",
            Category = ""
        };

        // Act
        var result = await product.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
        var errors = result.ToList();
        Assert.True(errors.Count >= 3); // 3 erros
    }

    /// <summary>
    /// Tests that category validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate category field")]
    public async Task Given_EmptyCategory_When_Validated_Then_ShouldHaveCategoryError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.Category = "";

        // Act
        var result = await product.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, e => e.Detail.Contains("category", StringComparison.OrdinalIgnoreCase));
    }
}