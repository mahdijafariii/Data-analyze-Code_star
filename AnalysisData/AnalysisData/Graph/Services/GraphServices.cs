using AnalysisData.Repository.AccountRepository.Abstraction;

namespace AnalysisData.Graph.Services;

public class GraphServices
{
    private readonly IGraphUtility _graphUtility;
    private readonly IAccountRepository _accountRepository;

    public GraphServices(IGraphUtility graphUtility, IAccountRepository accountRepository)
    {
        _graphUtility = graphUtility;
        _accountRepository = accountRepository;
    }

    // public string GetAllAccountPagination(int page)
    // {
    //     // _accountRepository.
    // }
}