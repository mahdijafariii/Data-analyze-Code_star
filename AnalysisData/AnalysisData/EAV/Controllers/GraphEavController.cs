using AnalysisData.Data;
using AnalysisData.EAV.Service;
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
    public async Task<IActionResult> GetNodesAsync([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var paginatedNodes = await _graphServiceEav.GetNodesPaginationAsync(pageIndex, pageSize);
        return Ok(paginatedNodes);
    }
    
    [HttpGet("Search/GetNode")]
     public async Task<IActionResult> GetEntityNodeWithAttributes(string headerUniqueId)
     {
         var output = await _graphServiceEav.GetNodeInformation(headerUniqueId);
         return Ok(output);
     }
    
    [HttpGet("GetRelationalEdgeByNodeName")]
    public async Task<IActionResult> GetRelationalEdgeByNodeName([FromQuery] string nodeID)
    {
        var result = await _graphServiceEav.GetRelationalEdgeBaseNode(nodeID);
        return Ok(new
        {
            nodes = result.Item1,
            edges = result.Item2
        });
    }
}