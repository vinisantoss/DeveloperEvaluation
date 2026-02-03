using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;

/// <summary>
/// Command for retrieving a paginated list of commercial transactions
/// </summary>
public class ListTransactionsCommand : IRequest<ListTransactionsResult>
{
    /// <summary>
    /// Gets or sets the page number (1-based)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Initializes a new ListTransactionsCommand
    /// </summary>
    public ListTransactionsCommand()
    {
    }

    /// <summary>
    /// Initializes a new ListTransactionsCommand with pagination
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="size">Page size</param>
    public ListTransactionsCommand(int page, int size)
    {
        Page = page;
        Size = size;
    }
}