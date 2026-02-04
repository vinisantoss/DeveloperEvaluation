using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the BusinessPartner entity class.
/// </summary>
public class BusinessPartnerTests
{
    /// <summary>
    /// Tests that validation passes when all business partner properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid business partner data")]
    public async Task Given_ValidBusinessPartnerData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var businessPartner = CommercialTransactionTestData.GenerateValidBusinessPartner();

        // Act
        var result = await businessPartner.ValidateAsync();

        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    /// Tests that validation fails when business partner properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid business partner data")]
    public async Task Given_InvalidBusinessPartnerData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var businessPartner = new BusinessPartner
        {
            ExternalId = "", // Invalid: empty
            Name = "", // Invalid: empty
            Email = "invalid-email", // Invalid: not valid email
            Document = "" // Invalid: empty
        };

        // Act
        var result = await businessPartner.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
    }

    /// <summary>
    /// Tests that email validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate email format correctly")]
    public async Task Given_InvalidEmail_When_Validated_Then_ShouldHaveEmailError()
    {
        // Arrange
        var businessPartner = CommercialTransactionTestData.GenerateValidBusinessPartner();
        businessPartner.Email = "invalid-email";

        // Act
        var result = await businessPartner.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, e => e.Detail.Contains("email", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that required fields validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate required fields")]
    public async Task Given_EmptyRequiredFields_When_Validated_Then_ShouldHaveRequiredFieldErrors()
    {
        // Arrange
        var businessPartner = new BusinessPartner
        {
            ExternalId = "",
            Name = "",
            Document = ""
        };

        // Act
        var result = await businessPartner.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
        var errors = result.ToList();
        Assert.True(errors.Count >= 3); // 3 erros
    }
}