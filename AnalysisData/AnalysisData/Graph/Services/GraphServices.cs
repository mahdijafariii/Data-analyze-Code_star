using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;
using AnalysisData.Repository.AccountRepository.Abstraction;

namespace AnalysisData.Graph.Services;

public class GraphServices : IGraphService
{
    private readonly IGraphUtility _graphUtility;
    private readonly IAccountRepository _accountRepository;

    public GraphServices(IGraphUtility graphUtility, IAccountRepository accountRepository)
    {
        _graphUtility = graphUtility;
        _accountRepository = accountRepository;
    }

    public async Task<(int,List<PaginationDto>)> GetAllAccountPagination(int page)
    {
         var count = await _accountRepository.GetCountNodes();
         var accounts = await _accountRepository.GetAllAccountPagination(page);
         return (count, accounts);
    }
    
}