using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;

namespace AnalysisData.Repository.AccountRepository.Abstraction;

public interface IAccountRepository
{
    Task AddAccountsAsync(IEnumerable<Account> accounts);
    Task<List<Account>> GetAllAccountPagination(int page);
    Task<Account> GetSpecialNode(string id);
    Task<Account> GetAccountById(string id);
    Task<int> GetCountNodes();

}