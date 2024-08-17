using AnalysisData.Data;
using AnalysisData.DataManage.Model;
using AnalysisData.Repository.TrancsactionRepository.Abstraction;

namespace AnalysisData.Repository.TrancsactionRepository;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddTransactionsAsync(IEnumerable<Transaction> transactions)
    {
        _context.Transactions.AddRange(transactions);
        await _context.SaveChangesAsync();
    }
}