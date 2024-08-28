using AnalysisData.EAV.Service;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FileAccessController : ControllerBase
{
    private readonly IFilePermissionService _filePermissionService;
    
    public FileAccessController(IFilePermissionService filePermissionService)
    {
        _filePermissionService = filePermissionService;
    }
    
    [HttpGet("GetFileForAccessingFile")]
    public async Task<IActionResult> GetFilesAsync([FromQuery] int page = 0, [FromQuery] int limit = 10)
    {
        var paginatedFiles = await _filePermissionService.GetFilesPagination(page, limit);
        return Ok(paginatedFiles);
    }
        
    [HttpGet("GetUsersForAccessingFile")]
    public async Task<IActionResult> GetUsersAsync([FromQuery] string username)
    {
        var users = await _filePermissionService.GetUserForAccessingFile(username);
        return Ok(users);
    }
    
    [HttpPost("AccessFileToUser")]
    public async Task<IActionResult> AccessFileToUser([FromBody] List<string> userGuidIdes,int fileId)
    {
        await _filePermissionService.AccessFileToUser(userGuidIdes, fileId);
        return Ok(new 
        {
            massage = "success"
        });
    }
            
    [HttpGet("WhoAccessToThisFile")]
    public async Task<IActionResult> WhoAccessToThisFile([FromQuery] string fileId)
    {
        var result = await _filePermissionService.WhoAccessThisFile(fileId);
        return Ok(result);
    }
    
}