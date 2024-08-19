using AnalysisData.Graph.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Graph.Controller;

public class GraphController : ControllerBase
{
    private readonly IGraphUtility _graphUtility;
    private readonly IGraphService _graphService;

    public GraphController(IGraphUtility graphUtility, IGraphService graphService)
    {
        _graphUtility = graphUtility;
        _graphService = graphService;
    }
    [HttpGet("pagination")]
    public async Task<IActionResult> GetGraph([FromQuery] int page = 1)
    {
        var result = await _graphService.GetAllAccountPagination(page);
        return Ok(new
        {
            Accounts = result.Item1,
            page = result.Item2,
            Count = result.Item3
        });
    }

    [HttpGet("special-node")]
    public async Task<IActionResult> GetSpecialNode([FromQuery] string id)
    {
        var result = await _graphService.GetSpecialNode(id);
        return Ok(new
        {
            id = result.AccountID,
            firstName = result.OwnerName,
            lastName = result.OwnerLastName,
            other = new 
            {
                result.BranchAdress,
                result.BranchName,
                result.AccountType,
                result.BranchTelephone,
                result.IBAN,
                result.CardID
            }
        });
    }
    
    [HttpGet("get-node-transactions")]
    public async Task<IActionResult> GetNodeTransactions([FromQuery] string id)
    {
        var result = await _graphService.GetSpecialNode(id);
        return Ok(new
        {
            id = result.AccountID,
            firstName = result.OwnerName,
            lastName = result.OwnerLastName,
            other = new 
            {
                result.BranchAdress,
                result.BranchName,
                result.AccountType,
                result.BranchTelephone,
                result.IBAN,
                result.CardID
            }
        });
    }
    
    [HttpGet("get-transaction-base-on-node-id")]
    public async Task<IActionResult> GetTransactionBaseOnNodeId([FromQuery] string nodeId)
    {
        var result = await _graphService.GetTransactionBasedOnNodeId(nodeId);
        return Ok(new
        {
            Account = result.accounts,
            Tranasction = result.transactions
        });
    }
    
    [HttpGet("Search-Nodes-Contain-Name-And-Id")]
    public async Task<IActionResult> SearchNodeContainNameAndId([FromQuery] string searchInput)
    {
        var accountId = await _graphService.SearchNodesAsNameAndId(searchInput);
        return Ok(accountId);
    }
}