using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.EAV.Service.Business.Abstraction;

namespace AnalysisData.EAV.Service;

public class Edge2Db2DbService:IEdge2DBService
{
    private readonly IEdgeRecordProcessor _edgeRecordProcessor;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderService _csvReaderService;

    public Edge2Db2DbService(IEdgeRecordProcessor edgeRecordProcessor, IFromToProcessor fromToProcessor, ICsvReaderService csvReaderService)
    {
        _edgeRecordProcessor = edgeRecordProcessor;
        _fromToProcessor = fromToProcessor;
        _csvReaderService = csvReaderService;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string from, string to)
    {
        var csv=_csvReaderService.CreateCsvReader(file);
        var headers = _csvReaderService.ReadHeaders(csv);

        await _fromToProcessor.ProcessFromToAsync(headers, from, to);
        await _edgeRecordProcessor.ProcessRecordsAsync(csv, headers,from,to);
    }
}