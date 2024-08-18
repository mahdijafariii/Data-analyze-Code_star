using AnalysisData.Graph.DataManage.Model;

namespace AnalysisData.Repository.AccountRepository.Abstraction;

public interface IAccountRepository
{
    Task AddAccountsAsync(IEnumerable<Account> accounts);
    Task<IEnumerable<Account>> GetAllAccountPagination(int page);
    Task<Account> GetAccountById(string id);
    Task<int> GetAllAccountPagination();

}