using AnalysisData.DataManage.Model;

namespace AnalysisData.Repository.AccountRepository.Abstraction;

public interface IAccountRepository
{
    Task AddAccountsAsync(IEnumerable<Account> accounts);
}