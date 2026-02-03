using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Transactions.ListTransactions;

/// <summary>
/// Handler for processing ListTransactionsCommand requests
/// </summary>
public class ListTransactionsHandler : IRequestHandler<ListTransactionsCommand, ListTransactionsResult>
{
    private readonly ICommercialTransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListTransactionsHandler> _logger;

    /// <summary>
    /// Initializes a new instance of ListTransactionsHandler
    /// </summary>
    /// <param name="transactionRepository">The transaction repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public ListTransactionsHandler(
        ICommercialTransactionRepository transactionRepository,
        IMapper mapper,
        ILogger<ListTransactionsHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the ListTransactionsCommand request
    /// </summary>
    /// <param name="request">The ListTransactions command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of transactions</returns>
    public async Task<ListTransactionsResult> Handle(ListTransactionsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving transactions list - Page: {Page}, Size: {Size}", request.Page, request.Size);

        var transactions = await _transactionRepository.GetAllAsync(request.Page, request.Size, cancellationToken);
        var transactionsList = transactions.ToList();

        var totalCount = transactionsList.Count;
        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        var result = new ListTransactionsResult
        {
            Transactions = _mapper.Map<List<TransactionListItem>>(transactionsList),
            CurrentPage = request.Page,
            PageSize = request.Size,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        _logger.LogInformation("Retrieved {Count} transactions for page {Page}", transactionsList.Count, request.Page);

        return result;
    }
}