using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Repository.AccountRepository;
using AnalysisData.Repository.TransactionRepository;
using QuickGraph;

namespace AnalysisData.Graph;

public class GraphUtility
{
    public BidirectionalGraph<Account, TransactionEdgeAdapter> Graph { get; private set; }
    private readonly AccountRepository _accountService;
    private readonly TransactionRepository _transactionService;

    public GraphUtility(AccountRepository accountService, TransactionRepository transactionService)
    {
        Graph = new BidirectionalGraph<Account, TransactionEdgeAdapter>();
        _accountService = accountService;
        _transactionService = transactionService;
    }

    public async Task BuildGraphAsync()
    {
        var accounts = await _accountService.GetAllAccounts();
        foreach (var account in accounts)
        {
            if (!Graph.ContainsVertex(account))
            {
                Graph.AddVertex(account);
            }
        }

        var transactions = await _transactionService.GetAllTransaction();
        foreach (var transaction in transactions)
        {
            await AddTransactionAsync(transaction);
        }
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        var sourceAccount = await _accountService.GetAccountById(transaction.SourceAccount);
        var targetAccount = await _accountService.GetAccountById(transaction.DestinationAccount);

        if (sourceAccount == null || targetAccount == null)
        {
            throw new ArgumentException("Both accounts must exist in the database.");
        }

        var edge = new TransactionEdgeAdapter(sourceAccount, targetAccount, transaction);
        Graph.AddEdge(edge);
    }

    public string ToJson()
    {
        var transactions = new List<object>();

        foreach (var edge in Graph.Edges)
        {
            transactions.Add(new
            {
                From = edge.Source.AccountID,
                To = edge.Target.AccountID,
                Transaction = new
                {
                    SourceAccount = edge.Transaction.SourceAccount,
                    DestinationAccount = edge.Transaction.DestinationAccount,
                    Amount = edge.Transaction.Amount,
                    Date = edge.Transaction.Date,
                    TransactionID = edge.Transaction.TransactionID,
                    Type = edge.Transaction.Type
                }
            });
        }

        return System.Text.Json.JsonSerializer.Serialize(transactions, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
    }
}