using AnalysisData.Dtos.GraphDto.EdgeDto;
using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Exception.FileException;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;
using AnalysisData.Services.GraphService.FileUploadService.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers.GraphController;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly INodeToDbProcessor _nodeToDbProcessor;
    private readonly IEdgeToDbProcessor _edgeToDbProcessor;
    private readonly IUploadFileService _uploadFileService;


    public FileController(INodeToDbProcessor nodeToDbProcessor, IEdgeToDbProcessor edgeToDbProcessor,
        IUploadFileService uploadFileService)
    {
        _nodeToDbProcessor = nodeToDbProcessor;
        _edgeToDbProcessor = edgeToDbProcessor;
        _uploadFileService = uploadFileService;
    }

    [Authorize(Policy = "silver")]
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
            await _nodeToDbProcessor.ProcessCsvFileAsync(file, uniqueAttribute, fileId);

            return Ok(new
            {
                message = "nodes saved successfully in the database."
            });
        }
        catch (System.Exception e)
        {
            throw new FileProcessingException(e.Message);
        }
    }

    [Authorize(Policy = "silver")]
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
            await _edgeToDbProcessor.ProcessCsvFileAsync(file, from, to);
            return Ok(new
            {
                massage = "Edges saved successfully in the database."
            });
        }
        catch (System.Exception e)
        {
            throw new FileProcessingException(e.Message);
        }
    }
}