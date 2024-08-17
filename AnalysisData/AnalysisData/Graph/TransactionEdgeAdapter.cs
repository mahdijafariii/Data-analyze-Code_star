using AnalysisData.Graph.DataManage.Model;
using QuickGraph;

namespace AnalysisData.Graph;

public class TransactionEdgeAdapter : IEdge<Account>
{
    public Account Source { get; }
    public Account Target { get; }
    public Transaction Transaction { get; }

    public TransactionEdgeAdapter(Account source, Account target, Transaction transaction)
    {
        Source = source;
        Target = target;
        Transaction = transaction;
    }
}