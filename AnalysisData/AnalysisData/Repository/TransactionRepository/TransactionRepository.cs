using AnalysisData.Data;
using AnalysisData.DataManage.Model;
using AnalysisData.Repository.TransactionRepository.Abstraction;

namespace AnalysisData.Repository.TransactionRepository;

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