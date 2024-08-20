using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    //private readonly IFileManagementService _fileManagementService;
    private readonly INode2DBService _node2DbService;
    private readonly IEdge2DBService _edge2DbService;

    
    public FileController(INode2DBService node2DbService,IEdge2DBService edge2DbService)
    {
        _node2DbService = node2DbService;
        _edge2DbService = edge2DbService;
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

        if (file == null || file.Length == 0 || uniqueAttribute == null)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            await _node2DbService.ProcessCsvFileAsync(file, uniqueAttribute, file.FileName); 
            return Ok("Node account saved successfully in the database."); 
        }
        catch (System.Exception e)
        {
            return StatusCode(400, $"An error occurred while processing the file: {e.Message}");
        }
    }
    
    
    [HttpPost("upload-file-edge")]
    public async Task<IActionResult> UploadEdgeFile([FromForm] EdgeUploadDto edgeUploadDto)
    {
        var from = edgeUploadDto.From;
        var to = edgeUploadDto.To;
        var file = edgeUploadDto.File;

        if (file == null || file.Length == 0 || from == null || to==null)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            await _edge2DbService.ProcessCsvFileAsync(file, from,to); 
            return Ok("Node account saved successfully in the database."); 
        }
        catch (System.Exception e)
        {
            return StatusCode(400, $"An error occurred while processing the file: {e.Message}");
        }
    }
}