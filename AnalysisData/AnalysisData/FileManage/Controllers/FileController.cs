using AnalysisData.FileManage.Dto;
using AnalysisData.FileManage.Service;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.FileManage.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    //private readonly IFileManagementService _fileManagementService;
    private readonly INodeService _nodeService;
    
    public FileController(INodeService nodeService)
    {
        _nodeService = nodeService;
    }
    
    /*[HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty or not provided.");
        }

        var user = User;
        var stream = file.OpenReadStream();
        var result = await _fileManagementService.FileUpload(user, stream);

        return Ok(new
        {
            headres = result.Item1,
            recordNumber = result.Item2
        });
    }*/

    [HttpPost("upload-file-node")]
    public async Task<IActionResult> UploadNodeFile([FromForm] NodeUploadDto nodeUpload)
    {
        var uniqueAttribute = nodeUpload.Header;
        var file = nodeUpload.File;

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            await _nodeService.ProcessCsvFileAsync(file, uniqueAttribute); 
            return Ok("Node account saved successfully in the database."); 
        }
        catch (System.Exception e)
        {
            return StatusCode(400, $"An error occurred while processing the file: {e.Message}");
        }
    }
    
    
    
}