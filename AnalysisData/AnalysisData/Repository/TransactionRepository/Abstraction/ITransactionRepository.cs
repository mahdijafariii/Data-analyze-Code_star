using AnalysisData.DataManage.Model;

namespace AnalysisData.Repository.TrancsactionRepository.Abstraction;

public interface ITransactionRepository
{
    Task AddTransactionsAsync(IEnumerable<Transaction> transactions);
}