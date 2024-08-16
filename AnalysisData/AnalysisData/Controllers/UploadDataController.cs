using AnalysisData.DataProcessService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers;

[ApiController]
[Authorize(Roles = "admin")]
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
        await _dataProcessor.ProcessDataAsync(stream, "account");

        return Ok("Account data processed successfully.");
    }

    [HttpPost("upload/transaction")]
    public async Task<IActionResult> UploadTransactionFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty or not provided.");
        }

        var stream = file.OpenReadStream();
        await _dataProcessor.ProcessDataAsync(stream, "transaction");

        return Ok("Transaction data processed successfully.");
    }
}