using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.EdgeManager;

public class EdgeToDbProcessor : IEdgeToDbProcessor
{
    private readonly IEntityEdgeRecordProcessor _entityEdgeRecordProcessor;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderManager _csvReaderManager;

    public EdgeToDbProcessor(
        IFromToProcessor fromToProcessor,
        ICsvReaderManager csvReaderManager,
        IEntityEdgeRecordProcessor entityEdgeRecordProcessor)
    {
        _fromToProcessor = fromToProcessor;
        _csvReaderManager = csvReaderManager;
        _entityEdgeRecordProcessor = entityEdgeRecordProcessor;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string from, string to)
    {
        var csv = _csvReaderManager.CreateCsvReader(file);
        var requiredHeaders = new List<string> { from, to };
        
        var headers = _csvReaderManager.ReadHeaders(csv, requiredHeaders);
        
        var headerswithId = await _fromToProcessor.ProcessFromToAsync(headers, from, to);
        
        await _entityEdgeRecordProcessor.ProcessEdgesAsync(csv, headerswithId, from, to);
        
    }
}
