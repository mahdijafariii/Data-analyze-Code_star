using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.EAV.Service.Business.Abstraction;

namespace AnalysisData.EAV.Service;

public class EdgeService:IEdgeService
{
    private IEdgeRecordProcessor _edgeRecordProcessor;
    private IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderService _csvReaderService;

    public EdgeService(IEdgeRecordProcessor edgeRecordProcessor, IFromToProcessor fromToProcessor, ICsvReaderService csvReaderService)
    {
        _edgeRecordProcessor = edgeRecordProcessor;
        _fromToProcessor = fromToProcessor;
        _csvReaderService = csvReaderService;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string from = "From", string to = "To")
    {
        var csv=_csvReaderService.CreateCsvReader(file);
        var headers = _csvReaderService.ReadHeaders(csv);

        await _fromToProcessor.ProcessFromToAsync(headers, from, to);
        await _edgeRecordProcessor.ProcessRecordsAsync(csv, headers,from,to);
    }
}