using AnalysisData.Data;
using AnalysisData.DataManage.Model;
using AnalysisData.Repository.AccountRepository.Abstraction;

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
}