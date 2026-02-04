using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CreateCommercialTransaction;

/// <summary>
/// Provides methods for generating test data for CreateCommercialTransaction using the Bogus library.
/// </summary>
public static class CreateCommercialTransactionHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateCommercialTransactionCommand.
    /// </summary>
    private static readonly Faker<CreateCommercialTransactionCommand> CommandFaker = new Faker<CreateCommercialTransactionCommand>()
        .RuleFor(c => c.TransactionCode, f => f.Commerce.Product().Replace(" ", "").ToUpper())
        .RuleFor(c => c.BusinessPartner, f => new BusinessPartnerInfo
        {
            ExternalId = f.Random.AlphaNumeric(10),
            Name = f.Company.CompanyName(),
            Email = f.Internet.Email(),
            Document = f.Random.Replace("###########")
        })
        .RuleFor(c => c.OperationalUnit, f => new OperationalUnitInfo
        {
            ExternalId = f.Random.AlphaNumeric(10),
            Name = f.Address.City(),
            Location = f.Address.FullAddress()
        })
        .RuleFor(c => c.Items, f => GenerateValidItems(f.Random.Int(1, 5)));

    /// <summary>
    /// Generates a valid CreateCommercialTransactionCommand.
    /// </summary>
    public static CreateCommercialTransactionCommand GenerateValidCommand()
    {
        return CommandFaker.Generate();
    }

    /// <summary>
    /// Generates a command with invalid data (empty transaction code).
    /// </summary>
    public static CreateCommercialTransactionCommand GenerateInvalidCommand()
    {
        var command = GenerateValidCommand();
        command.TransactionCode = ""; // -> Invalid
        return command;
    }

    /// <summary>
    /// Generates a command with too many identical items.
    /// </summary>
    public static CreateCommercialTransactionCommand GenerateCommandWithTooManyItems()
    {
        var command = GenerateValidCommand();
        command.Items.First().Quantity = 25; // -> Invalid: more than 20
        return command;
    }

    /// <summary>
    /// Generates valid transaction items.
    /// </summary>
    private static List<TransactionItemInfo> GenerateValidItems(int count)
    {
        var faker = new Faker();
        var items = new List<TransactionItemInfo>();

        for (int i = 0; i < count; i++)
        {
            items.Add(new TransactionItemInfo
            {
                Product = new CommercialProductInfo
                {
                    ExternalId = faker.Random.AlphaNumeric(10),
                    Name = faker.Commerce.ProductName(),
                    Category = faker.Commerce.Categories(1).First(),
                    StandardPrice = faker.Random.Decimal(1, 100)
                },
                Quantity = faker.Random.Int(1, 20),
                ItemPrice = faker.Random.Decimal(1, 50)
            });
        }

        return items;
    }
}