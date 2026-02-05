using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.Common;
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
    public void Given_ValidProductData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();

        // Act
        var result = product.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when product properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid product data")]
    public void Given_InvalidProductData_When_Validated_Then_ShouldReturnInvalid()
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
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that standard price validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate standard price is positive")]
    public void Given_NegativePrice_When_Validated_Then_ShouldHavePriceError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.StandardPrice = -10m;

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("price", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that required fields validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate required fields")]
    public void Given_EmptyRequiredFields_When_Validated_Then_ShouldHaveRequiredFieldErrors()
    {
        // Arrange
        var product = new Product
        {
            ExternalId = "",
            Name = "",
            Category = "",
            StandardPrice = 10m // Valid price to focus on other fields
        };

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.Count() >= 3); // 3 required field errors
    }

    /// <summary>
    /// Tests that category validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate category field")]
    public void Given_EmptyCategory_When_Validated_Then_ShouldHaveCategoryError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.Category = "";

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("Category", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that external ID validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate external ID field")]
    public void Given_EmptyExternalId_When_Validated_Then_ShouldHaveExternalIdError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.ExternalId = "";

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("External", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that name validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate name field")]
    public void Given_EmptyName_When_Validated_Then_ShouldHaveNameError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.Name = "";

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("Name", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that zero price validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate zero price")]
    public void Given_ZeroPrice_When_Validated_Then_ShouldHavePriceError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.StandardPrice = 0m;

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("price", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that product can be created with valid data.
    /// </summary>
    [Fact(DisplayName = "Should create product with valid data")]
    public void Given_ValidProductData_When_Created_Then_ShouldSetAllProperties()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = Guid.NewGuid(), // Explicitly set ID
            ExternalId = "PROD001",
            Name = "Test Product",
            Category = "Electronics",
            StandardPrice = 99.99m
        };

        // Assert
        Assert.Equal("PROD001", product.ExternalId);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal("Electronics", product.Category);
        Assert.Equal(99.99m, product.StandardPrice);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    /// <summary>
    /// Tests that product can be created and ID can be set.
    /// </summary>
    [Fact(DisplayName = "Should allow setting product ID")]
    public void Given_Product_When_SettingId_Then_ShouldStoreCorrectly()
    {
        // Arrange
        var product = new Product();
        var expectedId = Guid.NewGuid();

        // Act
        product.Id = expectedId;
        product.ExternalId = "PROD001";
        product.Name = "Test Product";
        product.Category = "Electronics";
        product.StandardPrice = 99.99m;

        // Assert
        Assert.Equal(expectedId, product.Id);
        Assert.Equal("PROD001", product.ExternalId);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal("Electronics", product.Category);
        Assert.Equal(99.99m, product.StandardPrice);
    }

    /// <summary>
    /// Tests that product properties can be set correctly.
    /// </summary>
    [Fact(DisplayName = "Should set product properties correctly")]
    public void Given_ProductData_When_SettingProperties_Then_ShouldSetCorrectly()
    {
        // Arrange
        var product = new Product();
        var externalId = "PROD001";
        var name = "Test Product";
        var category = "Electronics";
        var price = 99.99m;

        // Act
        product.ExternalId = externalId;
        product.Name = name;
        product.Category = category;
        product.StandardPrice = price;

        // Assert
        Assert.Equal(externalId, product.ExternalId);
        Assert.Equal(name, product.Name);
        Assert.Equal(category, product.Category);
        Assert.Equal(price, product.StandardPrice);
    }

    /// <summary>
    /// Tests that product can be created with empty constructor.
    /// </summary>
    [Fact(DisplayName = "Should create product with empty constructor")]
    public void Given_EmptyConstructor_When_Creating_Then_ShouldCreateSuccessfully()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.NotNull(product);
        Assert.Equal(string.Empty, product.ExternalId);
        Assert.Equal(string.Empty, product.Name);
        Assert.Equal(string.Empty, product.Category);
        Assert.Equal(0m, product.StandardPrice);
    }

    /// <summary>
    /// Tests that product with all valid data passes validation.
    /// </summary>
    [Fact(DisplayName = "Should pass validation with all valid fields")]
    public void Given_AllValidFields_When_Validated_Then_ShouldPassValidation()
    {
        // Arrange
        var product = new Product
        {
            ExternalId = "PROD001",
            Name = "Test Product",
            Category = "Electronics",
            StandardPrice = 99.99m
        };

        // Act
        var result = product.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that very high price validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate maximum price limit")]
    public void Given_VeryHighPrice_When_Validated_Then_ShouldHavePriceError()
    {
        // Arrange
        var product = CommercialTransactionTestData.GenerateValidProduct();
        product.StandardPrice = 1000000m; // Above limit

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("price", StringComparison.OrdinalIgnoreCase));
    }
}