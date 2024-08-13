using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.JwtService.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class FindActionRoleController : ControllerBase
{
    [HttpGet("{role}")]
    public IActionResult GetActionsByRole(string role)
    {
        var actions = ActionRole.GetActionByRole(role);
        
        if (actions.Count == 0)
        {
            return NotFound(new { Message = "Role not found for this cockie." });
        }
        return Ok(actions);
    }
}