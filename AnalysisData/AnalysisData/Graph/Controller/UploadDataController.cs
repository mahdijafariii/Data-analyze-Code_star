using AnalysisData.Graph.DataProcessService;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Graph.Controller;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly IDataProcessor _dataProcessor;

    public DataController(IDataProcessor dataProcessor)
    {
        _dataProcessor = dataProcessor;
    }

    [HttpPost("upload/account")]
    public async Task<IActionResult> UploadAccountFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty or not provided.");
        }

        var stream = file.OpenReadStream();
        var result = await _dataProcessor.ProcessDataAsync(stream, "account");

        return Ok(result);
    }

    [HttpPost("upload/transaction")]
    public async Task<IActionResult> UploadTransactionFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty or not provided.");
        }

        var stream = file.OpenReadStream();
        var result = await _dataProcessor.ProcessDataAsync(stream, "account");

        return Ok(result);
    }
}