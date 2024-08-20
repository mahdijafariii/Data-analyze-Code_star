using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;

namespace AnalysisData.Repository.AccountRepository.Abstraction;

public interface IAccountRepository
{
    Task AddAccountsAsync(IEnumerable<Account> accounts);
    Task<List<Account>> GetAllAccountPagination(int page , int limit);
    Task<Account> GetSpecialNode(string id);
    Task<Account> GetAccountById(string id);
    Task<int> GetCountNodes();
    Task<List<Account>> GetAccountsWithTransactionIdes(List<string> ides);
    Task<IEnumerable<Account>> SearchNodesAsNameAndId(string search);

}