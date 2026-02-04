using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.GetTransaction;

/// <summary>
/// Provides methods for generating test data for GetTransaction using the Bogus library.
/// </summary>
public static class GetTransactionHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid GetTransactionCommand.
    /// </summary>
    private static readonly Faker<GetTransactionCommand> CommandFaker = new Faker<GetTransactionCommand>()
         .CustomInstantiator(f => new GetTransactionCommand(
            f.Random.Guid()
        ));

    /// <summary>
    /// Generates a valid GetTransactionCommand.
    /// </summary>
    public static GetTransactionCommand GenerateValidCommand()
    {
        return CommandFaker.Generate();
    }

    /// <summary>
    /// Generates a command with empty ID.
    /// </summary>
    public static GetTransactionCommand GenerateInvalidCommand()
    {
        return new GetTransactionCommand(Guid.Empty);
    }
}