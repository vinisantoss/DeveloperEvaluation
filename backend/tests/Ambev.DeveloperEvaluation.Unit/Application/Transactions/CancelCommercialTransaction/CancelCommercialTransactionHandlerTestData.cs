using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CancelTransaction;

/// <summary>
/// Provides methods for generating test data for CancelTransaction using the Bogus library.
/// </summary>
public static class CancelTransactionTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CancelTransactionCommand.
    /// </summary>
    private static readonly Faker<CancelTransactionCommand> CommandFaker = new Faker<CancelTransactionCommand>()
         .CustomInstantiator(f => new CancelTransactionCommand(f.Random.Guid()));

    /// <summary>
    /// Generates a valid CancelTransactionCommand.
    /// </summary>
    public static CancelTransactionCommand GenerateValidCommand()
    {
        return CommandFaker.Generate();
    }

    /// <summary>
    /// Generates a command with empty ID.
    /// </summary>
    public static CancelTransactionCommand GenerateInvalidCommand()
    {
        return new CancelTransactionCommand(Guid.Empty);
    }
}