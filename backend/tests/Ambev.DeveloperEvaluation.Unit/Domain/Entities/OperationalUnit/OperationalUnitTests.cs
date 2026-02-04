using Ambev.DeveloperEvaluation.Domain.Entities;
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
    public async Task Given_ValidOperationalUnitData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var operationalUnit = CommercialTransactionTestData.GenerateValidOperationalUnit();

        // Act
        var result = await operationalUnit.ValidateAsync();

        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    /// Tests that validation fails when operational unit properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid operational unit data")]
    public async Task Given_InvalidOperationalUnitData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var operationalUnit = new OperationalUnit
        {
            ExternalId = "", // Invalid: empty
            Name = "", // Invalid: empty
            Location = "" // Invalid: empty
        };

        // Act
        var result = await operationalUnit.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
    }

    /// <summary>
    /// Tests that required fields validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate required fields")]
    public async Task Given_EmptyRequiredFields_When_Validated_Then_ShouldHaveRequiredFieldErrors()
    {
        // Arrange
        var operationalUnit = new OperationalUnit
        {
            ExternalId = "",
            Name = "",
            Location = ""
        };

        // Act
        var result = await operationalUnit.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
        var errors = result.ToList();
        Assert.True(errors.Count >= 3); // 3 erros
    }

    /// <summary>
    /// Tests that name validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Should validate name field")]
    public async Task Given_EmptyName_When_Validated_Then_ShouldHaveNameError()
    {
        // Arrange
        var operationalUnit = CommercialTransactionTestData.GenerateValidOperationalUnit();
        operationalUnit.Name = "";

        // Act
        var result = await operationalUnit.ValidateAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, e => e.Detail.Contains("name", StringComparison.OrdinalIgnoreCase));
    }
}