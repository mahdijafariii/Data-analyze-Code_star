using AnalysisData.EAV.Service;
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
    public async Task<IActionResult> GetNodesAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10,[FromQuery] string category = "all" )
    {
        var paginatedNodes = await _graphServiceEav.GetNodesPaginationAsync(pageIndex, pageSize , category);
        return Ok(paginatedNodes);
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