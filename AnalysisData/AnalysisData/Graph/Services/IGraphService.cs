using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;

namespace AnalysisData.Graph.Services;

public interface IGraphService
{
    Task<(List<PaginationDto>, int, int)> GetAllAccountPagination(int page = 0);
    Task<Account> GetSpecialNode(string id);

    Task<(IEnumerable<AccountContractDto> accounts, IEnumerable<TransactionsContractDto> transactions)>
        GetTransactionBasedOnNodeId(string id);
}