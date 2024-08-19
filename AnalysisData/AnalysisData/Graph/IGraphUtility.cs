using AnalysisData.Graph.DataManage.Model;
using QuickGraph;

namespace AnalysisData.Graph;

public interface IGraphUtility
{
    Task<BidirectionalGraph<Account,TransactionEdgeAdapter>> BuildGraphAsync(List<Account> accounts , List<Transaction> transactions);
    Task AddTransactionAsync(Transaction transaction);
    string ToJson();
    BidirectionalGraph<Account,TransactionEdgeAdapter> GetGraph(); 
}