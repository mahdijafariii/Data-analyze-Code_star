using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

namespace AnalysisData.Services.GraphService.Business.NodeManager;

public class NodeToDbProcessor : INodeToDbProcessor
{
    private readonly ICsvReaderManager _csvReaderManager;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly INodeRecordProcessor _nodeRecordProcessor;

    public NodeToDbProcessor(ICsvReaderManager csvReaderManager, IHeaderProcessor headerProcessor,
        INodeRecordProcessor nodeRecordProcessor)
    {
        _csvReaderManager = csvReaderManager;
        _headerProcessor = headerProcessor;
        _nodeRecordProcessor = nodeRecordProcessor;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string id, int fileId)
    {
        var csv = _csvReaderManager.CreateCsvReader(file);
        var headers = _csvReaderManager.ReadHeaders(csv, new List<string> { id });
        
        var headersWithId = await _headerProcessor.ProcessHeadersAsync(headers, id);
        
        
        await _nodeRecordProcessor.ProcessNodesAsync(csv, headersWithId, id, fileId);
    }
}