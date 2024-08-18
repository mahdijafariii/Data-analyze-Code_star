using AnalysisData.Data;
using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Repository.TransactionRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

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


    public async Task<IEnumerable<Transaction>> GetAllTransaction()
    {
        return await _context.Transactions.ToListAsync();
    }
    public async Task<HashSet<Transaction>> GetTransactionBasedOnNodeId(string nodeId)
    { 
        var result = await _context.Transactions.Where(x => x.SourceAccount == nodeId || x.DestinationAccount == nodeId).ToListAsync();
        return result.ToHashSet();
    }
    public async Task<Transaction> GetById(string id)
    {
        return await _context.Transactions.SingleOrDefaultAsync(x => x.TransactionID == id);
    }
}