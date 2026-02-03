using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Transactions.CancelTransaction;

/// <summary>
/// Command for cancelling a commercial transaction
/// </summary>
public class CancelTransactionCommand : IRequest<CancelTransactionResult>
{
    /// <summary>
    /// Gets or sets the transaction's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new CancelTransactionCommand
    /// </summary>
    /// <param name="id">The transaction ID</param>
    public CancelTransactionCommand(Guid id)
    {
        Id = id;
    }
}