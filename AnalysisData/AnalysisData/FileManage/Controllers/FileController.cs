using AnalysisData.FileManage.Service.Abstraction;
using AnalysisData.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.FileManage.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileManagementService _fileManagementService; 
    
    public FileController(IFileManagementService fileManagementService)
    {
        _fileManagementService = fileManagementService;
    }
    
    [HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty or not provided.");
        }

        var user = User;
        var stream = file.OpenReadStream();
        var csvHeader = await _fileManagementService.FileUpload(user, stream);

        return Ok(csvHeader);
    }
    
}