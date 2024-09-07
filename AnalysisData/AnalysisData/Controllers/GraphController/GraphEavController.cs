using AnalysisData.Services.GraphService.GraphServices.AllNodesData.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.Relationship.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.Search.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers.GraphController;

[ApiController]
[Route("api/[controller]")]
public class GraphEavController : ControllerBase
{
    private readonly INodePaginationService _nodePaginationService;
    private readonly IGraphSearchService _graphSearchService;
    private readonly IGraphRelationService _graphRelationService;
    private readonly INodeAndEdgeInfo _nodeAndEdgeInfo;

    public GraphEavController(INodePaginationService nodePaginationService, IGraphSearchService graphSearchService,
        IGraphRelationService graphRelationService, INodeAndEdgeInfo nodeAndEdgeInfo)
    {
        _nodePaginationService = nodePaginationService;
        _graphSearchService = graphSearchService;
        _graphRelationService = graphRelationService;
        _nodeAndEdgeInfo = nodeAndEdgeInfo;
    }

    [Authorize(Policy = "bronze")]
    [HttpGet("nodes")]
    public async Task<IActionResult> GetNodesAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10,
        [FromQuery] int? category = null)
    {
        var user = User;
        var paginatedNodes = await _nodePaginationService.GetAllNodesAsync(user, pageIndex, pageSize, category);
        return Ok(paginatedNodes);
    }

    [Authorize(Policy = "bronze")]
    [HttpGet("nodes/{nodeId}/attributes")]
    public async Task<IActionResult> GetNodeAttributes(int nodeId)
    {
        var user = User;
        var output = await _nodeAndEdgeInfo.GetNodeInformationAsync(user, nodeId);
        return Ok(output);
    }

    [Authorize(Policy = "bronze")]
    [HttpGet("edges/{edgeId}/attributes")]
    public async Task<IActionResult> GetEdgeAttribute(int edgeId)
    {
        var user = User;
        var output = await _nodeAndEdgeInfo.GetEdgeInformationAsync(user, edgeId);
        return Ok(output);
    }

    [Authorize(Policy = "bronze")]
    [HttpGet("nodes-relation")]
    public async Task<IActionResult> GetRelationalEdgeByNodeId([FromQuery] int nodeId)
    {
        var user = User;
        var result = await _graphRelationService.GetRelationalEdgeBaseNodeAsync(user, nodeId);
        return Ok(new
        {
            nodes = result.Item1,
            edges = result.Item2
        });
    }

    [Authorize(Policy = "bronze")]
    [HttpGet("Search")]
    public async Task<IActionResult> SearchEntityNode([FromQuery] string searchInput, string searchType = "contain",[FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {
        var user = User;
        var result = await _graphSearchService.SearchInEntityNodeNameAsync(user, searchInput, searchType,pageIndex,pageSize);
        return Ok(result);
    }
}