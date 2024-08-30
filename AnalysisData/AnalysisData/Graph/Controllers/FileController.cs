using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.Exception;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly INodeToDbService _nodeToDbService;
    private readonly IEdgeToDbService _edgeToDbService;
    private readonly IUploadFileService _uploadFileService;


    public FileController(INodeToDbService nodeToDbService, IEdgeToDbService edgeToDbService,
        IUploadFileService uploadFileService)
    {
        _nodeToDbService = nodeToDbService;
        _edgeToDbService = edgeToDbService;
        _uploadFileService = uploadFileService;
    }

    [HttpPost("upload-file-node")]
    public async Task<IActionResult> UploadNodeFile([FromForm] NodeUploadDto nodeUpload)
    {
        var user = User;
        var uniqueAttribute = nodeUpload.Header;
        var file = nodeUpload.File;
        var categoryId = nodeUpload.CategoryId;
        var name = nodeUpload.Name;
        if (file == null || file.Length == 0 || uniqueAttribute == null)
        {
            throw new NoFileUploadedException();
        }

        try
        {
            var fileId = await _uploadFileService.AddFileToDb(categoryId, user, name);
            await _nodeToDbService.ProcessCsvFileAsync(file, uniqueAttribute, fileId);

            return Ok(new
            {
                message = "File uploaded successfully!"
            });
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

        if (file == null || file.Length == 0 || from == null || to == null)
        {
            throw new NoFileUploadedException();
        }

        try
        {
            await _edgeToDbService.ProcessCsvFileAsync(file, from, to);
            return Ok(new
            {
                massage = "Edges saved successfully in the database."
            });
        }
        catch (System.Exception e)
        {
            return StatusCode(400, $"An error occurred while processing the file: {e.Message}");
        }
    }
}