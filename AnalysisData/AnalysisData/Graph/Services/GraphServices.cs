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

    public async Task<(List<PaginationDto>,int,int)> GetAllAccountPagination(int page = 0)
    {
        var count = await _accountRepository.GetCountNodes();
         var accounts = await _accountRepository.GetAllAccountPagination(page);
         var paginationData = accounts.Select(x => new PaginationDto()
             { AccountID = x.AccountID, OwnerName = x.OwnerName, OwnerLastName = x.OwnerLastName }).ToList();
         return (paginationData,page,count);
    }

    public async Task<Account> GetSpecialNode(string id)
    {
        return  await _accountRepository.GetSpecialNode(id);
    }
    
}