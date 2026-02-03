using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Transactions.GetTransaction;

/// <summary>
/// Command for retrieving a commercial transaction by its ID
/// </summary>
public class GetTransactionCommand : IRequest<GetTransactionResult>
{
    /// <summary>
    /// Gets or sets the transaction's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new GetTransactionCommand
    /// </summary>
    /// <param name="id">The transaction ID</param>
    public GetTransactionCommand(Guid id)
    {
        Id = id;
    }
}