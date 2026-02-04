using Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.ListTransactions;

/// <summary>
/// Provides methods for generating test data for ListTransactions using the Bogus library.
/// </summary>
public static class ListTransactionsTestData
{
    /// <summary>
    /// Configures the Faker to generate valid ListTransactionsCommand.
    /// </summary>
    private static readonly Faker<ListTransactionsCommand> CommandFaker = new Faker<ListTransactionsCommand>()
       .CustomInstantiator(f => new ListTransactionsCommand(
           f.Random.Int(1, 10),  // Page
           f.Random.Int(1, 50)   // Size
       ));

    /// <summary>
    /// Generates a valid ListTransactionsCommand.
    /// </summary>
    public static ListTransactionsCommand GenerateValidCommand()
    {
        return CommandFaker.Generate();
    }

    /// <summary>
    /// Generates a command with invalid pagination (negative page).
    /// </summary>
    public static ListTransactionsCommand GenerateInvalidCommand()
    {
        var command = GenerateValidCommand();
        command.Page = -1; // Invalid
        return command;
    }

    /// <summary>
    /// Generates a command with no filters.
    /// </summary>
    public static ListTransactionsCommand GenerateCommandWithoutFilters()
    {
        return new ListTransactionsCommand
        {
            Page = 1,
            Size = 10
        };
    }

    /// <summary>
    /// Generates a command with no pagination.
    /// </summary>
    public static ListTransactionsCommand GenerateCommandWithPagination(int page, int size)
    {
        return new ListTransactionsCommand(page, size);
    }
}