using AnalysisData.Data;

using AnalysisData.EAV.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraphEavController : ControllerBase
{
    private readonly IGraphServiceEav _graphServiceEav;

    public GraphEavController(IGraphServiceEav graphServiceEav)
    {
        _graphServiceEav = graphServiceEav;
    }

    [HttpGet("GetNodesPaginationEav")]
    public async Task<IActionResult> GetNodesAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, [FromQuery] int? category = null)
    {
        var user = User;
        var paginatedNodes = await _graphServiceEav.GetNodesPaginationAsync(user,pageIndex, pageSize, category);
        return Ok(paginatedNodes);
    }

    [HttpGet("GetNodeInformation")]
    public async Task<IActionResult> GetEntityNodeWithAttributes(string headerUniqueId)
    {
        var user = User;
        var output = await _graphServiceEav.GetNodeInformation(user,headerUniqueId);
        return Ok(output);
    }

    [HttpGet("GetEdgeInformation")]
    public async Task<IActionResult> GetEntityEdgeWithAttributes(int edgeId)
    {
        var user = User;
        var output = await _graphServiceEav.GetEdgeInformation(user,edgeId);
        return Ok(output);
    }

    [HttpGet("GetRelationalEdgeByNodeId")]
    public async Task<IActionResult> GetRelationalEdgeByNodeName([FromQuery] string nodeId)
    {
        var user = User;
        var result = await _graphServiceEav.GetRelationalEdgeBaseNode(user,nodeId);
        return Ok(new
        {
            nodes = result.Item1,
            edges = result.Item2
        });
    }
    
    [HttpGet("Search")]
    public async Task<IActionResult> SearchEntityNode([FromQuery] string searchInput, string searchType = "contain")
    {
        var result = await _graphServiceEav.SearchEntityNodeName(searchInput,searchType);
        return Ok(result);
    }
}