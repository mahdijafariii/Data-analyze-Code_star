using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Repository.AccountRepository;
using AnalysisData.Repository.AccountRepository.Abstraction;
using AnalysisData.Repository.TransactionRepository;
using AnalysisData.Repository.TransactionRepository.Abstraction;
using QuickGraph;

namespace AnalysisData.Graph;

public class GraphUtility : IGraphUtility
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly BidirectionalGraph<Account, TransactionEdgeAdapter> _graph;

    public GraphUtility(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _graph = new BidirectionalGraph<Account, TransactionEdgeAdapter>();
    }

    public async Task<BidirectionalGraph<Account,TransactionEdgeAdapter>> BuildGraphAsync(List<Account> accounts , List<Transaction> transactions)
    {
        foreach (var account in accounts)
        {
            if (!_graph.ContainsVertex(account))
            {
                _graph.AddVertex(account);
            }
        }
        foreach (var transaction in transactions)
        {
            await AddTransactionAsync(transaction);
        }
        return _graph;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        var sourceAccount = await _accountRepository.GetAccountById(transaction.SourceAccount);
        var targetAccount = await _accountRepository.GetAccountById(transaction.DestinationAccount);

        if (sourceAccount == null || targetAccount == null)
        {
            throw new ArgumentException("Both accounts must exist in the database.");
        }

        if (!_graph.ContainsVertex(sourceAccount))
        {
            _graph.AddVertex(sourceAccount);
        }

        if (!_graph.ContainsVertex(targetAccount))
        {
            _graph.AddVertex(targetAccount);
        }

        var edge = new TransactionEdgeAdapter(sourceAccount, targetAccount, transaction);
        _graph.AddEdge(edge);
    }

    public BidirectionalGraph<Account, TransactionEdgeAdapter> GetGraph()
    {
        return _graph;
    }

    public string ToJson()
    {
        var edges = new List<object>();

        foreach (var edge in _graph.Edges)
        {
            edges.Add(new
            {
                From = edge.Source.AccountID,
                To = edge.Target.AccountID,
                Label = edge is TransactionEdgeAdapter txEdge ? txEdge.ToString() : "N/A"
            });
        }

        return System.Text.Json.JsonSerializer.Serialize(edges,
            new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
    }
}