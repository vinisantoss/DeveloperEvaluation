using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using Bogus;


namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CancelTransactionItem;

/// <summary>
/// Provides methods for generating test data for CancelTransactionItem using the Bogus library.
/// </summary>
public static class CancelTransactionItemHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CancelTransactionItemCommand.
    /// </summary>
    private static readonly Faker<CancelTransactionItemCommand> CommandFaker = new Faker<CancelTransactionItemCommand>()
       .CustomInstantiator(f => new CancelTransactionItemCommand(
           f.Random.Guid(), // TransactionId
           f.Random.Guid()  // ItemId
       ));

    /// <summary>
    /// Generates a valid CancelTransactionItemCommand.
    /// </summary>
    public static CancelTransactionItemCommand GenerateValidCommand()
    {
        return CommandFaker.Generate();
    }

    /// <summary>
    /// Generates a command with empty IDs.
    /// </summary>
    public static CancelTransactionItemCommand GenerateInvalidCommand()
    {
        return new CancelTransactionItemCommand(Guid.Empty, Guid.Empty);
    }
}