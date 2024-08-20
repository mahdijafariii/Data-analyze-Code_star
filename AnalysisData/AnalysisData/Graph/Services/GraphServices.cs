using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;
using AnalysisData.Repository.AccountRepository.Abstraction;
using AnalysisData.Repository.TransactionRepository.Abstraction;
using Transaction = System.Transactions.Transaction;

namespace AnalysisData.Graph.Services;

public class GraphServices : IGraphServices
{
    private readonly IGraphUtility _graphUtility;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public GraphServices(IGraphUtility graphUtility, IAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        _graphUtility = graphUtility;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<(List<PaginationDto>, int, int)> GetAllAccountPagination(int page, int limit)
    {
        var count = await _accountRepository.GetCountNodes();
        var accounts = await _accountRepository.GetAllAccountPagination(page, limit);
        var paginationData = accounts.Select(x => new PaginationDto()
            { AccountID = x.AccountID, OwnerName = x.OwnerName, OwnerLastName = x.OwnerLastName }).ToList();
        return (paginationData, page, count);
    }

    public async Task<Account> GetSpecialNode(string id)
    {
        return await _accountRepository.GetSpecialNode(id);
    }

    public async Task<(IEnumerable<AccountContractDto> accounts, IEnumerable<TransactionsContractDto> transactions)>
        GetTransactionBasedOnNodeId(string id)
    {
        var transactions = await _transactionRepository.GetTransactionBasedOnNodeId(id);
        var uniqueAccounts = transactions
            .SelectMany(x => new[] { x.SourceAccount, x.DestinationAccount })
            .Distinct()
            .ToList();
        var accounts = await _accountRepository.GetAccountsWithTransactionIdes(uniqueAccounts);
        var accountsDto = accounts.Select(x => new AccountContractDto()
            { Id = x.AccountID, Lable = $"{x.OwnerName} {x.OwnerLastName}, {x.BranchName}" });
        var transactionDto = transactions.Select(x => new TransactionsContractDto()
            { From = x.SourceAccount, To = x.DestinationAccount, AomuntOfMoney = $"{x.Amount}" });
        return (accountsDto, transactionDto);
    }


    public async Task<IEnumerable<string>> SearchNodesAsNameAndId(string searchInput)
    {
        var accounts = await _accountRepository.SearchNodesAsNameAndId(searchInput);
        var accountIdes = accounts.Select(x => x.AccountID);
        return accountIdes;
    }
}