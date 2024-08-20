using AnalysisData.Graph.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Graph.Controller;

public class GraphController : ControllerBase
{
    private readonly IGraphUtility _graphUtility;
    private readonly IGraphServices _graphServices;

    public GraphController(IGraphUtility graphUtility, IGraphServices graphServices)
    {
        _graphUtility = graphUtility;
        _graphServices = graphServices;
    }
    [HttpGet("get-nodes-pagination")]
    public async Task<IActionResult> GetGraph([FromQuery] int page = 1 , int limit = 10)
    {
        var result = await _graphServices.GetAllAccountPagination(page , limit);
        return Ok(new
        {
            Accounts = result.Item1,
            page = result.Item2,
            Count = result.Item3
        });
    }

    [HttpGet("special-node-information")]
    public async Task<IActionResult> GetSpecialNodeInformation([FromQuery] string id)
    {
        var result = await _graphServices.GetSpecialNode(id);
        return Ok(result);
    }

    [HttpGet("get-transaction-of-special-node")]
    public async Task<IActionResult> GetTransactionBaseOnNodeId([FromQuery] string nodeId)
    {
        var result = await _graphServices.GetTransactionBasedOnNodeId(nodeId);
        return Ok(new
        {
            Account = result.accounts,
            Tranasction = result.transactions
        });
    }
    
    [HttpGet("Search-Nodes-Contain-Name-And-Id")]
    public async Task<IActionResult> SearchNodeContainNameAndId([FromQuery] string searchInput)
    {
        var accountId = await _graphServices.SearchNodesAsNameAndId(searchInput);
        return Ok(accountId);
    }
}