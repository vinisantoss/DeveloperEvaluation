using Ambev.DeveloperEvaluation.Application.Transactions.CreateCommercialTransaction;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CreateCommercialTransaction;

/// <summary>
/// Provides methods for generating test data for CreateCommercialTransaction using the Bogus library.
/// </summary>
public static class CreateCommercialTransactionHandlerTestData
{
    /// <summary>
    /// Generates a valid CreateCommercialTransactionCommand with safe data.
    /// </summary>
    public static CreateCommercialTransactionCommand GenerateValidCommand()
    {
        var faker = new Faker();

        return new CreateCommercialTransactionCommand
        {
            TransactionCode = faker.Commerce.Product().Replace(" ", "").ToUpper(),
            BusinessPartner = new BusinessPartnerInfo
            {
                ExternalId = faker.Random.AlphaNumeric(10),
                Name = faker.Company.CompanyName(),
                Email = faker.Internet.Email(),
                Document = faker.Random.Replace("###########")
            },
            OperationalUnit = new OperationalUnitInfo
            {
                ExternalId = faker.Random.AlphaNumeric(10),
                Name = faker.Address.City(),
                Location = faker.Address.FullAddress()
            },
            Items = GenerateValidItems(faker.Random.Int(1, 3)) // Max 3 diff
        };
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
        var faker = new Faker();

        return new CreateCommercialTransactionCommand
        {
            TransactionCode = faker.Commerce.Product().Replace(" ", "").ToUpper(),
            BusinessPartner = new BusinessPartnerInfo
            {
                ExternalId = faker.Random.AlphaNumeric(10),
                Name = faker.Company.CompanyName(),
                Email = faker.Internet.Email(),
                Document = faker.Random.Replace("###########")
            },
            OperationalUnit = new OperationalUnitInfo
            {
                ExternalId = faker.Random.AlphaNumeric(10),
                Name = faker.Address.City(),
                Location = faker.Address.FullAddress()
            },
            Items = new List<TransactionItemInfo>
            {
                new TransactionItemInfo
                {
                    Product = new CommercialProductInfo
                    {
                        ExternalId = "PROD001",
                        Name = "Test Product",
                        Category = "Test Category",
                        StandardPrice = 10.00m
                    },
                    Quantity = 25, // -> Invalid: more than 20
                    ItemPrice = 10.00m
                }
            }
        };
    }

    /// <summary>
    /// Generates valid transaction items with guaranteed safe quantities.
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
                    ExternalId = $"PROD{i + 1:D3}", //
                    Name = $"Product {i + 1}",
                    Category = faker.Commerce.Categories(1).First(),
                    StandardPrice = faker.Random.Decimal(1, 100)
                },
                Quantity = faker.Random.Int(1, 10), // 
                ItemPrice = faker.Random.Decimal(1, 50)
            });
        }

        return items;
    }

    /// <summary>
    /// Generates a command specifically for testing grand total calculation.
    /// </summary>
    public static CreateCommercialTransactionCommand GenerateCommandForGrandTotalTest()
    {
        return new CreateCommercialTransactionCommand
        {
            TransactionCode = "TEST001",
            BusinessPartner = new BusinessPartnerInfo
            {
                ExternalId = "BP001",
                Name = "Test Business Partner",
                Email = "test@test.com",
                Document = "12345678901"
            },
            OperationalUnit = new OperationalUnitInfo
            {
                ExternalId = "OU001",
                Name = "Test Operational Unit",
                Location = "Test Location"
            },
            Items = new List<TransactionItemInfo>
            {
                new TransactionItemInfo
                {
                    Product = new CommercialProductInfo
                    {
                        ExternalId = "PROD001",
                        Name = "Product 1",
                        Category = "Category 1",
                        StandardPrice = 10.00m
                    },
                    Quantity = 5, // 5 items
                    ItemPrice = 10.00m // Total: 50.00
                },
                new TransactionItemInfo
                {
                    Product = new CommercialProductInfo
                    {
                        ExternalId = "PROD002", 
                        Name = "Product 2",
                        Category = "Category 2",
                        StandardPrice = 20.00m
                    },
                    Quantity = 3, // 3 items
                    ItemPrice = 20.00m // Total: 60.00
                }
                // Grand Total: 110.00
            }
        };
    }

    /// <summary>
    /// Generates a command with exactly 20 items (boundary test).
    /// </summary>
    public static CreateCommercialTransactionCommand GenerateCommandWithExactly20Items()
    {
        var faker = new Faker();

        return new CreateCommercialTransactionCommand
        {
            TransactionCode = faker.Commerce.Product().Replace(" ", "").ToUpper(),
            BusinessPartner = new BusinessPartnerInfo
            {
                ExternalId = faker.Random.AlphaNumeric(10),
                Name = faker.Company.CompanyName(),
                Email = faker.Internet.Email(),
                Document = faker.Random.Replace("###########")
            },
            OperationalUnit = new OperationalUnitInfo
            {
                ExternalId = faker.Random.AlphaNumeric(10),
                Name = faker.Address.City(),
                Location = faker.Address.FullAddress()
            },
            Items = new List<TransactionItemInfo>
            {
                new TransactionItemInfo
                {
                    Product = new CommercialProductInfo
                    {
                        ExternalId = "PROD001",
                        Name = "Test Product",
                        Category = "Test Category",
                        StandardPrice = 10.00m
                    },
                    Quantity = 20, 
                    ItemPrice = 10.00m
                }
            }
        };
    }

    /// <summary>
    /// Generates a simple command with minimal valid data.
    /// </summary>
    public static CreateCommercialTransactionCommand GenerateSimpleValidCommand()
    {
        return new CreateCommercialTransactionCommand
        {
            TransactionCode = "SIMPLE001",
            BusinessPartner = new BusinessPartnerInfo
            {
                ExternalId = "BP001",
                Name = "Simple Business Partner",
                Email = "simple@test.com",
                Document = "12345678901"
            },
            OperationalUnit = new OperationalUnitInfo
            {
                ExternalId = "OU001",
                Name = "Simple Operational Unit",
                Location = "Simple Location"
            },
            Items = new List<TransactionItemInfo>
            {
                new TransactionItemInfo
                {
                    Product = new CommercialProductInfo
                    {
                        ExternalId = "PROD001",
                        Name = "Simple Product",
                        Category = "Simple Category",
                        StandardPrice = 10.00m
                    },
                    Quantity = 1, 
                    ItemPrice = 10.00m
                }
            }
        };
    }
}