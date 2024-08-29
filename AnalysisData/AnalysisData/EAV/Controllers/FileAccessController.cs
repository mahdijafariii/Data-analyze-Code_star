using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Service;
using AnalysisData.Exception;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FileAccessController : ControllerBase
{
    private readonly IFilePermissionService _filePermissionService;
    private readonly IUserFileRepository _userFileRepository;
    
    public FileAccessController(IFilePermissionService filePermissionService, IUserFileRepository userFileRepository)
    {
        _filePermissionService = filePermissionService;
        _userFileRepository = userFileRepository;
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
    public async Task<IActionResult> AccessFileToUser([FromBody] AccessFileToUserDto request)
    {
        try
        {
            await _filePermissionService.AccessFileToUser(request.UserGuidIds.ToList(), request.FileId);
            return Ok(new  
            {
                message = "success"
            });
        }
        catch (GuidNotCorrectFormat ex)
        {
            return BadRequest(new  
            {
                message = ex.Message
            });
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new  
            {
                message = ex.Message
            });
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new  
            {
                message = ex.Message
            });
        }
    }
            
    [HttpGet("WhoAccessToThisFile")]
    public async Task<IActionResult> WhoAccessToThisFile([FromQuery] int fileId)
    {
        var file = await _userFileRepository.GetByFileIdAsync(fileId);
        if (file is null)
        {
            throw new FileNotFoundException();
        }
        var result = await _filePermissionService.WhoAccessThisFile(fileId);
        return Ok(result);
    }
    
}