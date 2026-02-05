using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for BusinessPartner using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class BusinessPartnerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid BusinessPartner entities.
    /// The generated business partners will have valid:
    /// - ExternalId (using alphanumeric codes)
    /// - Name (using company names)
    /// - Email (valid format)
    /// - Document (Brazilian CNPJ format)
    /// </summary>
    private static readonly Faker<BusinessPartner> BusinessPartnerFaker = new Faker<BusinessPartner>()
        .RuleFor(bp => bp.ExternalId, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(bp => bp.Name, f => f.Company.CompanyName())
        .RuleFor(bp => bp.Email, f => f.Internet.Email())
        .RuleFor(bp => bp.Document, f => f.Random.Replace("##############")); // 14 digits for CNPJ

    /// <summary>
    /// Generates a valid BusinessPartner entity with randomized data.
    /// The generated business partner will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid BusinessPartner entity with randomly generated data.</returns>
    public static BusinessPartner GenerateValidBusinessPartner()
    {
        return BusinessPartnerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid email address using Faker.
    /// The generated email will:
    /// - Follow the standard email format (user@domain.com)
    /// - Have valid characters in both local and domain parts
    /// - Have a valid TLD
    /// </summary>
    /// <returns>A valid email address.</returns>
    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email();
    }

    /// <summary>
    /// Generates a valid external ID for business partner.
    /// The generated external ID will:
    /// - Be alphanumeric
    /// - Have appropriate length (10 characters)
    /// - Be in uppercase format
    /// </summary>
    /// <returns>A valid external ID.</returns>
    public static string GenerateValidExternalId()
    {
        return new Faker().Random.AlphaNumeric(10).ToUpper();
    }

    /// <summary>
    /// Generates a valid company name.
    /// The generated name will:
    /// - Be a realistic company name
    /// - Have appropriate length
    /// - Contain valid characters
    /// </summary>
    /// <returns>A valid company name.</returns>
    public static string GenerateValidName()
    {
        return new Faker().Company.CompanyName();
    }

    /// <summary>
    /// Generates a valid document number (CNPJ format).
    /// The generated document will:
    /// - Have 14 digits
    /// - Follow Brazilian CNPJ format
    /// </summary>
    /// <returns>A valid document number.</returns>
    public static string GenerateValidDocument()
    {
        return new Faker().Random.Replace("##############"); // 14 digits
    }

    /// <summary>
    /// Generates an invalid email address for testing negative scenarios.
    /// The generated email will:
    /// - Not follow the standard email format
    /// - Not contain the @ symbol
    /// - Be a simple word or string
    /// This is useful for testing email validation error cases.
    /// </summary>
    /// <returns>An invalid email address.</returns>
    public static string GenerateInvalidEmail()
    {
        var faker = new Faker();
        return faker.Lorem.Word(); // Simple word without @ or .
    }

    /// <summary>
    /// Generates an invalid external ID for testing negative scenarios.
    /// The generated external ID will:
    /// - Be empty
    /// This is useful for testing external ID validation error cases.
    /// </summary>
    /// <returns>An invalid external ID.</returns>
    public static string GenerateInvalidExternalId()
    {
        return ""; // Empty string
    }

    /// <summary>
    /// Generates an invalid name for testing negative scenarios.
    /// The generated name will:
    /// - Be empty
    /// This is useful for testing name validation error cases.
    /// </summary>
    /// <returns>An invalid name.</returns>
    public static string GenerateInvalidName()
    {
        return ""; // Empty string
    }

    /// <summary>
    /// Generates an invalid document for testing negative scenarios.
    /// The generated document will:
    /// - Be too short (less than 11 characters)
    /// This is useful for testing document validation error cases.
    /// </summary>
    /// <returns>An invalid document.</returns>
    public static string GenerateInvalidDocument()
    {
        return new Faker().Random.AlphaNumeric(5); // Too short - only 5 characters
    }

    /// <summary>
    /// Generates a name that exceeds the maximum length limit.
    /// The generated name will:
    /// - Be longer than 100 characters
    /// - Contain random characters
    /// This is useful for testing name length validation error cases.
    /// </summary>
    /// <returns>A name that exceeds the maximum length limit.</returns>
    public static string GenerateLongName()
    {
        return new Faker().Random.String2(101); // 101 characters - exceeds max length
    }

    /// <summary>
    /// Generates an external ID that exceeds the maximum length limit.
    /// The generated external ID will:
    /// - Be longer than 50 characters
    /// - Contain random characters
    /// This is useful for testing external ID length validation error cases.
    /// </summary>
    /// <returns>An external ID that exceeds the maximum length limit.</returns>
    public static string GenerateLongExternalId()
    {
        return new Faker().Random.String2(51); // 51 characters - exceeds max length
    }
}