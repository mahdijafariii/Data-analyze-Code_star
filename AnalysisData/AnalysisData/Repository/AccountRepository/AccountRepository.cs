using AnalysisData.Data;
using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Repository.AccountRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repository.AccountRepository;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAccountsAsync(IEnumerable<Account> accounts)
    {
        _context.Accounts.AddRange(accounts);
        await _context.SaveChangesAsync();
    }
    
    
    public async Task<IEnumerable<Account>> GetAllAccountPagination(int page)
    {
        return await _context.Accounts.Skip(10*page).Take(10).ToListAsync();
    }
    
    public async Task<int> GetAllAccountPagination()
    {
        return await _context.Accounts.CountAsync();
    }
    public async Task<Account> GetAccountById(string id)
    {
        return await _context.Accounts.SingleOrDefaultAsync(x => x.AccountID == id);
    }
}