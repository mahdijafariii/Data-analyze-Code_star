using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;

namespace AnalysisData.Repository.AccountRepository.Abstraction;

public interface IAccountRepository
{
    Task AddAccountsAsync(IEnumerable<Account> accounts);
    Task<List<PaginationDto>> GetAllAccountPagination(int page);
    Task<Account> GetAccountById(string id);
    Task<int> GetCountNodes();

}