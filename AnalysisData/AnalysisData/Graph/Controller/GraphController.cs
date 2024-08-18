using AnalysisData.Graph.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Graph.Controller;

public class GraphController : ControllerBase
{
    private readonly IGraphUtility _graphUtility;
    private readonly IGraphService _graphService;

    public GraphController(IGraphUtility graphUtility,IGraphService graphService)
    {
        _graphUtility = graphUtility;
        _graphService = graphService;
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetGraph([FromQuery] int page)
    {
        var result = await _graphService.GetAllAccountPagination(page);
        return Ok(new
        {
            Count = result.Item1,
            Accounts = result.Item2
        });
    }
}