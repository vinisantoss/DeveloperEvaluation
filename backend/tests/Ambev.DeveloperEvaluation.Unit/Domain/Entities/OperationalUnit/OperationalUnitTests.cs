using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Transactions.Common;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the OperationalUnit entity class.
/// </summary>
public class OperationalUnitTests
{
    /// <summary>
    /// Tests that validation passes when all operational unit properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid operational unit data")]
    public void Given_ValidOperationalUnitData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var operationalUnit = CommercialTransactionTestData.GenerateValidOperationalUnit();

        // Act
        var result = operationalUnit.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when operational unit properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid operational unit data")]
    public void Given_InvalidOperationalUnitData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var operationalUnit = new OperationalUnit
        {
            ExternalId = "",
            Name = "",
            Location = ""
        };

        // Act
        var result = operationalUnit.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that required fields validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate required fields")]
    public void Given_EmptyRequiredFields_When_Validated_Then_ShouldHaveRequiredFieldErrors()
    {
        // Arrange
        var operationalUnit = new OperationalUnit
        {
            ExternalId = "",
            Name = "",
            Location = ""
        };

        // Act
        var result = operationalUnit.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.Count() >= 3);
    }

    /// <summary>
    /// Tests that name validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate name field")]
    public void Given_EmptyName_When_Validated_Then_ShouldHaveNameError()
    {
        // Arrange
        var operationalUnit = CommercialTransactionTestData.GenerateValidOperationalUnit();
        operationalUnit.Name = "";

        // Act
        var result = operationalUnit.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains("Name", StringComparison.OrdinalIgnoreCase));
    }
}