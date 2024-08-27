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
        var paginatedNodes = await _graphServiceEav.GetNodesPaginationAsync(pageIndex, pageSize, category);
        return Ok(paginatedNodes);
    }

    [HttpGet("GetNodeInformation")]
    public async Task<IActionResult> GetEntityNodeWithAttributes(string headerUniqueId)
    {
        var output = await _graphServiceEav.GetNodeInformation(headerUniqueId);
        return Ok(output);
    }

    [HttpGet("GetEdgeInformation")]
    public async Task<IActionResult> GetEntityEdgeWithAttributes(int edgeId)
    {
        var output = await _graphServiceEav.GetEdgeInformation(edgeId);
        return Ok(output);
    }

    [HttpGet("GetRelationalEdgeByNodeName")]
    public async Task<IActionResult> GetRelationalEdgeByNodeName([FromQuery] string nodeId)
    {
        var result = await _graphServiceEav.GetRelationalEdgeBaseNode(nodeId);
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