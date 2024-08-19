using AnalysisData.Data;
using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;
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


    public async Task<List<Account>> GetAllAccountPagination(int page)
    {
        return await _context.Accounts.Skip(10 * page).Take(10).ToListAsync();
    }

    public async Task<List<PaginationDto>> SearchAccountPagination(int page)
    {
        return await _context.Accounts.Skip(10 * page).Take(10).Select(x => new PaginationDto()
            { AccountID = x.AccountID, OwnerName = x.OwnerName, OwnerLastName = x.OwnerLastName }).ToListAsync();
    }

    public async Task<Account> GetSpecialNode(string id)
    {
        return await _context.Accounts.SingleOrDefaultAsync(x => x.AccountID == id);
    }


    public async Task<List<Account>> GetAccountsWithTransactionIdes(List<string> ides)
    {
        List<Account> accounts = new List<Account>();
        foreach (var id in ides)
        {
            var account = await GetSpecialNode(id);
            if (account != null)
            {
                accounts.Add(account);
            }
        }

        return accounts;
    }

    public async Task<int> GetCountNodes()
    {
        return await _context.Accounts.CountAsync();
    }

    public async Task<IEnumerable<Account>> SearchNodesAsNameAndId(string search)
    {
        return await _context.Accounts.Where(x => x.AccountID.Contains(search) || x.OwnerName.Contains(search))
            .ToListAsync();
    }

    public async Task<Account> GetAccountById(string id)
    {
        return await _context.Accounts.SingleOrDefaultAsync(x => x.AccountID == id);
    }
}