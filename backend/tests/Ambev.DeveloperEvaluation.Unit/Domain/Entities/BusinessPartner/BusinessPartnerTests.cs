using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the BusinessPartner entity class.
/// Tests cover validation scenarios for business partner data.
/// </summary>
public class BusinessPartnerTests
{
    /// <summary>
    /// Tests that validation passes when all business partner properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid business partner data")]
    public void Given_ValidBusinessPartnerData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var businessPartner = BusinessPartnerTestData.GenerateValidBusinessPartner();

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when business partner properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid business partner data")]
    public void Given_InvalidBusinessPartnerData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var businessPartner = new BusinessPartner
        {
            ExternalId = "", // Invalid: empty
            Name = "", // Invalid: empty
            Email = BusinessPartnerTestData.GenerateInvalidEmail(), // Invalid: not valid email
            Document = "" // Invalid: empty
        };

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that email validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate email format correctly")]
    public void Given_InvalidEmail_When_Validated_Then_ShouldHaveEmailError()
    {
        // Arrange
        var businessPartner = BusinessPartnerTestData.GenerateValidBusinessPartner();
        businessPartner.Email = BusinessPartnerTestData.GenerateInvalidEmail();

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Detail.Contains("email", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that required fields validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate required fields")]
    public void Given_EmptyRequiredFields_When_Validated_Then_ShouldHaveRequiredFieldErrors()
    {
        // Arrange
        var businessPartner = new BusinessPartner
        {
            ExternalId = "",
            Name = "",
            Document = "",
            Email = BusinessPartnerTestData.GenerateValidEmail() 
        };

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.Count() >= 3); 
    }

    /// <summary>
    /// Tests that external ID validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate external ID correctly")]
    public void Given_EmptyExternalId_When_Validated_Then_ShouldHaveExternalIdError()
    {
        // Arrange
        var businessPartner = BusinessPartnerTestData.GenerateValidBusinessPartner();
        businessPartner.ExternalId = "";

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("External ID", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that name validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate name correctly")]
    public void Given_EmptyName_When_Validated_Then_ShouldHaveNameError()
    {
        // Arrange
        var businessPartner = BusinessPartnerTestData.GenerateValidBusinessPartner();
        businessPartner.Name = "";

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("Name", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that document validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate document correctly")]
    public void Given_EmptyDocument_When_Validated_Then_ShouldHaveDocumentError()
    {
        // Arrange
        var businessPartner = BusinessPartnerTestData.GenerateValidBusinessPartner();
        businessPartner.Document = "";

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("Document", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that document length validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate document length correctly")]
    public void Given_ShortDocument_When_Validated_Then_ShouldHaveDocumentLengthError()
    {
        // Arrange
        var businessPartner = BusinessPartnerTestData.GenerateValidBusinessPartner();
        businessPartner.Document = BusinessPartnerTestData.GenerateInvalidDocument(); 

        // Act
        var result = businessPartner.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("Document", StringComparison.OrdinalIgnoreCase));
    }
}