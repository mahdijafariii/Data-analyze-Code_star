using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Graph.Controller;

public class GraphController : ControllerBase
{
    private readonly IGraphUtility _graphUtility;
    public GraphController(IGraphUtility graphUtility)
    {
        _graphUtility = graphUtility;
    }

    // [HttpGet("graph")]
    // public async Task<IActionResult> GetGraph()
    // {
    //     await _graphUtility.BuildGraphAsync();
    //     var jsonResult = _graphUtility.ToJson();
    //     return Ok(jsonResult);
    // }
}