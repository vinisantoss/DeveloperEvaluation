using Ambev.DeveloperEvaluation.Application.Transactions.CancelTransactionItem;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Transactions.CancelTransactionItem;

/// <summary>
/// Provides methods for generating test data for CancelTransactionItem using the Bogus library.
/// </summary>
public static class CancelTransactionItemHandlerTestData
{
    /// <summary>
    /// Generates a valid CancelTransactionItemCommand.
    /// </summary>
    public static CancelTransactionItemCommand GenerateValidCommand()
    {
        return new CancelTransactionItemCommand(
            Guid.NewGuid(), // TransactionId
            Guid.NewGuid()  // ItemId
        );
    }

    /// <summary>
    /// Generates a command with specific IDs for controlled testing.
    /// </summary>
    public static CancelTransactionItemCommand GenerateCommandWithIds(Guid transactionId, Guid itemId)
    {
        return new CancelTransactionItemCommand(transactionId, itemId);
    }

    /// <summary>
    /// Generates a command with empty IDs for validation testing.
    /// </summary>
    public static CancelTransactionItemCommand GenerateInvalidCommand()
    {
        return new CancelTransactionItemCommand(Guid.Empty, Guid.Empty);
    }

    /// <summary>
    /// Generates a command with empty transaction ID.
    /// </summary>
    public static CancelTransactionItemCommand GenerateCommandWithEmptyTransactionId()
    {
        return new CancelTransactionItemCommand(Guid.Empty, Guid.NewGuid());
    }

    /// <summary>
    /// Generates a command with empty item ID.
    /// </summary>
    public static CancelTransactionItemCommand GenerateCommandWithEmptyItemId()
    {
        return new CancelTransactionItemCommand(Guid.NewGuid(), Guid.Empty);
    }
}