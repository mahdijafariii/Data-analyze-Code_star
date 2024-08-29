using AnalysisData.Data;

using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.EAV.Service.GraphServices.NodeAndEdgeServices;
using AnalysisData.EAV.Service.GraphServices.Relationship;
using AnalysisData.EAV.Service.GraphSevices;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraphEavController : ControllerBase
{
    private readonly INodePaginationService _nodePaginationService;
    private readonly IGraphSearchService _graphSearchService;
    private readonly IGraphRelationService _graphRelationService;
    private readonly INodeAndEdgeInfo _nodeAndEdgeInfo;

    public GraphEavController(INodePaginationService nodePaginationService, IGraphSearchService graphSearchService,IGraphRelationService graphRelationService,INodeAndEdgeInfo nodeAndEdgeInfo)
    {
        _nodePaginationService = nodePaginationService;
        _graphSearchService = graphSearchService;
        _graphRelationService = graphRelationService;
        _nodeAndEdgeInfo = nodeAndEdgeInfo;
    }

    [HttpGet("GetNodesPaginationEav")]
    public async Task<IActionResult> GetNodesAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, [FromQuery] int? category = null)
    {
        var user = User;
        var paginatedNodes = await _nodePaginationService.GetAllNodesAsync(user,pageIndex, pageSize, category);
        return Ok(paginatedNodes);
    }

    [HttpGet("GetNodeInformation")]
    public async Task<IActionResult> GetEntityNodeWithAttributes(string headerUniqueId)
    {
        var user = User;
        var output = await _nodeAndEdgeInfo.GetNodeInformationAsync(user,headerUniqueId);
        return Ok(output);
    }

    [HttpGet("GetEdgeInformation")]
    public async Task<IActionResult> GetEntityEdgeWithAttributes(int edgeId)
    {
        var user = User;
        var output = await _nodeAndEdgeInfo.GetEdgeInformationAsync(user,edgeId);
        return Ok(output);
    }

    [HttpGet("GetRelationalEdgeByNodeId")]
    public async Task<IActionResult> GetRelationalEdgeByNodeName([FromQuery] string nodeId)
    {
        var user = User;
        var result = await _graphRelationService.GetRelationalEdgeBaseNodeAsync(user,nodeId);
        return Ok(new
        {
            nodes = result.Item1,
            edges = result.Item2
        });
    }
    
    [HttpGet("Search")]
    public async Task<IActionResult> SearchEntityNode([FromQuery] string searchInput, string searchType = "contain")
    {
        var user = User;
        var result = await _graphSearchService.SearchInEntityNodeNameAsync(user,searchInput,searchType);
        return Ok(result);
    }
}