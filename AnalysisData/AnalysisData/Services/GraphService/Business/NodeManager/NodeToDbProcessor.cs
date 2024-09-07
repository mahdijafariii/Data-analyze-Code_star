using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

namespace AnalysisData.Services.GraphService.Business.NodeManager;

public class NodeToDbProcessor : INodeToDbProcessor
{
    private readonly ICsvReaderManager _csvReaderManager;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly INodeRecordProcessor _nodeRecordProcessor;
    private readonly IValueNodeProcessor _valueNodeProcessor;

    public NodeToDbProcessor(ICsvReaderManager csvReaderManager, IHeaderProcessor headerProcessor,
        INodeRecordProcessor nodeRecordProcessor, IValueNodeProcessor valueNodeProcessor)
    {
        _csvReaderManager = csvReaderManager;
        _headerProcessor = headerProcessor;
        _nodeRecordProcessor = nodeRecordProcessor;
        _valueNodeProcessor = valueNodeProcessor;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string id, int fileId)
    {
        var csv = _csvReaderManager.CreateCsvReader(file);
        var headers = _csvReaderManager.ReadHeaders(csv, new List<string> { id });
        
        await _headerProcessor.ProcessHeadersAsync(headers, id);
        
        csv = _csvReaderManager.CreateCsvReader(file);
        headers = _csvReaderManager.ReadHeaders(csv, new List<string> { id });
        
        var entityNodes = await _nodeRecordProcessor.ProcessEntityNodesAsync(csv, headers, id, fileId);
        
        csv = _csvReaderManager.CreateCsvReader(file);
        headers = _csvReaderManager.ReadHeaders(csv, new List<string> { id });
        await _valueNodeProcessor.ProcessValueNodesAsync(csv, entityNodes, headers, id);
    }
}