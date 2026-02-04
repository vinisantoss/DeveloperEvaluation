using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for CommercialTransaction using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CommercialTransactionTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CommercialTransaction entities.
    /// </summary>
    private static readonly Faker<CommercialTransaction> TransactionFaker = new Faker<CommercialTransaction>()
        .RuleFor(t => t.Id, f => f.Random.Guid())
        .RuleFor(t => t.TransactionCode, f => f.Commerce.Product().Replace(" ", "").ToUpper())
        .RuleFor(t => t.TransactionDate, f => f.Date.Recent(30))
        .RuleFor(t => t.Status, f => TransactionStatus.Active)
        .RuleFor(t => t.CreatedAt, f => f.Date.Recent(30))
        .RuleFor(t => t.UpdatedAt, f => null)
        .RuleFor(t => t.BusinessPartnerId, f => f.Random.Guid())
        .RuleFor(t => t.OperationalUnitId, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid BusinessPartner entities.
    /// </summary>
    private static readonly Faker<BusinessPartner> BusinessPartnerFaker = new Faker<BusinessPartner>()
        .RuleFor(bp => bp.Id, f => f.Random.Guid())
        .RuleFor(bp => bp.ExternalId, f => f.Random.AlphaNumeric(10))
        .RuleFor(bp => bp.Name, f => f.Company.CompanyName())
        .RuleFor(bp => bp.Email, f => f.Internet.Email())
        .RuleFor(bp => bp.Document, f => f.Random.Replace("###########"));

    /// <summary>
    /// Configures the Faker to generate valid OperationalUnit entities.
    /// </summary>
    private static readonly Faker<OperationalUnit> OperationalUnitFaker = new Faker<OperationalUnit>()
        .RuleFor(ou => ou.Id, f => f.Random.Guid())
        .RuleFor(ou => ou.ExternalId, f => f.Random.AlphaNumeric(10))
        .RuleFor(ou => ou.Name, f => f.Address.City())
        .RuleFor(ou => ou.Location, f => f.Address.FullAddress());

    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// </summary>
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.ExternalId, f => f.Random.AlphaNumeric(10))
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
        .RuleFor(p => p.StandardPrice, f => f.Random.Decimal(1, 100));

    /// <summary>
    /// Configures the Faker to generate valid TransactionItem entities.
    /// </summary>
    private static readonly Faker<TransactionItem> TransactionItemFaker = new Faker<TransactionItem>()
        .RuleFor(ti => ti.Id, f => f.Random.Guid())
        .RuleFor(ti => ti.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(ti => ti.ItemPrice, f => f.Random.Decimal(1, 50))
        .RuleFor(ti => ti.DiscountPercentage, f => 0)
        .RuleFor(ti => ti.IsCancelled, f => false)
        .RuleFor(ti => ti.Product, f => ProductFaker.Generate())
        .RuleFor(ti => ti.ProductId, (f, ti) => ti.Product?.Id ?? f.Random.Guid());

    /// <summary>
    /// Generates a valid CommercialTransaction entity with randomized data.
    /// </summary>
    /// <returns>A valid CommercialTransaction entity with randomly generated data.</returns>
    public static CommercialTransaction GenerateValidTransaction()
    {
        var faker = new Faker();
        var transaction = TransactionFaker.Generate();

        var businessPartner = BusinessPartnerFaker.Generate();
        var operationalUnit = OperationalUnitFaker.Generate();

        transaction.BusinessPartner = businessPartner;
        transaction.BusinessPartnerId = businessPartner.Id;

        transaction.OperationalUnit = operationalUnit;
        transaction.OperationalUnitId = operationalUnit.Id;

        var scenario = faker.Random.Int(1, 3);
        transaction.Items = new List<TransactionItem>();

        switch (scenario)
        {
            case 1: // 1-3 items, no discount
                CreateItemsWithoutDiscount(transaction, faker.Random.Int(1, 3));
                break;

            case 2: // 4-9 items, 10% discount
                CreateItemsWithDiscount(transaction, faker.Random.Int(4, 9), 10m);
                break;

            case 3: // 10-20 items, 20% discount
                CreateItemsWithDiscount(transaction, faker.Random.Int(10, 20), 20m);
                break;
        }

        transaction.Amount = transaction.Items.Sum(i => i.ItemTotal);
        return transaction;
    }

    private static void CreateItemsWithoutDiscount(CommercialTransaction transaction, int totalQuantity)
    {
        var product = ProductFaker.Generate();
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Product = product,
            TransactionId = transaction.Id,
            Quantity = totalQuantity,
            ItemPrice = new Faker().Random.Decimal(1, 50),
            DiscountPercentage = 0m,
            IsCancelled = false
        };

        item.ItemTotal = item.Quantity * item.ItemPrice;
        transaction.Items.Add(item);
    }

    private static void CreateItemsWithDiscount(CommercialTransaction transaction, int totalQuantity, decimal discount)
    {
        var product = ProductFaker.Generate();
        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Product = product,
            TransactionId = transaction.Id,
            Quantity = totalQuantity,
            ItemPrice = new Faker().Random.Decimal(1, 50),
            DiscountPercentage = discount, 
            IsCancelled = false
        };

        item.ItemTotal = item.Quantity * item.ItemPrice * (1 - item.DiscountPercentage / 100);
        transaction.Items.Add(item);
    }

    /// <summary>
    /// Generates items following discount rules
    /// </summary>
    private static List<TransactionItem> GenerateValidItemsWithDiscountRules()
    {
        var faker = new Faker();
        var items = new List<TransactionItem>();

        var totalQuantity = faker.Random.Int(4, 15);
        var itemCount = faker.Random.Int(1, 3); 

        for (int i = 0; i < itemCount; i++)
        {
            var product = ProductFaker.Generate();
            var quantity = i == 0 ? totalQuantity - (itemCount - 1) : 1;

            var item = new TransactionItem
            {
                Id = faker.Random.Guid(),
                ProductId = product.Id,
                Product = product,
                Quantity = Math.Max(1, quantity),
                ItemPrice = faker.Random.Decimal(1, 50),
                IsCancelled = false
            };

            items.Add(item);
        }

       
        var totalItems = items.Sum(i => i.Quantity);
        decimal discountPercentage = totalItems switch
        {
            >= 4 and < 10 => 10m,  // 4-9 items = 10%
            >= 10 and <= 20 => 20m, // 10-20 items = 20%
            _ => 0m // < 4 items = 0%
        };

        foreach (var item in items)
        {
            item.DiscountPercentage = discountPercentage;
        }

        return items;
    }

    /// <summary>
    /// Generates a valid BusinessPartner entity.
    /// </summary>
    public static BusinessPartner GenerateValidBusinessPartner()
    {
        return BusinessPartnerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid OperationalUnit entity.
    /// </summary>
    public static OperationalUnit GenerateValidOperationalUnit()
    {
        return OperationalUnitFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Product entity.
    /// </summary>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates a valid TransactionItem entity.
    /// </summary>
    public static TransactionItem GenerateValidTransactionItem()
    {
        var product = GenerateValidProduct();

        var item = new TransactionItem
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id, 
            Product = product,
            Quantity = new Faker().Random.Int(1, 20),
            ItemPrice = new Faker().Random.Decimal(1, 50),
            IsCancelled = false
        };

        item.DiscountPercentage = item.Quantity switch
        {
            < 4 => 0m,
            >= 4 and < 10 => 10m,
            >= 10 and <= 20 => 20m,
            _ => 0m
        };

        item.ItemTotal = item.Quantity * item.ItemPrice * (1 - item.DiscountPercentage / 100);

        return item;
    }

    /// <summary>
    /// Generates a transaction with more than 20 identical items (invalid scenario).
    /// </summary>
    public static CommercialTransaction GenerateTransactionWithTooManyItems()
    {
        var transaction = GenerateValidTransaction();
        transaction.Items.First().Quantity = 25; // Invalid: more than 20
        return transaction;
    }

    /// <summary>
    /// Generates a transaction with cancelled status.
    /// </summary>
    public static CommercialTransaction GenerateCancelledTransaction()
    {
        var transaction = GenerateValidTransaction();
        transaction.Status = TransactionStatus.Cancelled;
        return transaction;
    }

    /// <summary>
    /// Generates invalid transaction code (empty).
    /// </summary>
    public static string GenerateInvalidTransactionCode()
    {
        return string.Empty;
    }

    /// <summary>
    /// Generates invalid item quantity (zero or negative).
    /// </summary>
    public static int GenerateInvalidQuantity()
    {
        return new Faker().Random.Int(-5, 0);
    }

    /// <summary>
    /// Generates invalid item price (zero or negative).
    /// </summary>
    public static decimal GenerateInvalidPrice()
    {
        return new Faker().Random.Decimal(-10, 0);
    }
}