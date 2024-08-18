using AnalysisData.Graph.DataManage.Model;

namespace AnalysisData.Repository.TransactionRepository.Abstraction;

public interface ITransactionRepository
{
    Task AddTransactionsAsync(IEnumerable<Transaction> transactions);
    Task<IEnumerable<Transaction>> GetAllTransaction();
    Task<Transaction> GetById(string id);
}