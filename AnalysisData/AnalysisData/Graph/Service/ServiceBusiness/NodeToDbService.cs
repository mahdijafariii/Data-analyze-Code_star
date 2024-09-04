using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class NodeToDbService : INodeToDbService
{
    private readonly ICsvReaderService _csvReaderService;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly INodeRecordProcessor _nodeRecordProcessor;
    private readonly IValueNodeProcessor _valueNodeProcessor;

    public NodeToDbService(ICsvReaderService csvReaderService, IHeaderProcessor headerProcessor,
        INodeRecordProcessor nodeRecordProcessor, IValueNodeProcessor valueNodeProcessor)
    {
        _csvReaderService = csvReaderService;
        _headerProcessor = headerProcessor;
        _nodeRecordProcessor = nodeRecordProcessor;
        _valueNodeProcessor = valueNodeProcessor;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string id, int fileId)
    {
        var requiredHeaders = new List<string> { id };
    
        var csv = _csvReaderService.CreateCsvReader(file);
        var headers = _csvReaderService.ReadHeaders(csv, requiredHeaders);
    
        await _headerProcessor.ProcessHeadersAsync(headers, id);
        
        csv = _csvReaderService.CreateCsvReader(file);
        headers = _csvReaderService.ReadHeaders(csv, requiredHeaders);
    
        var entityNodes = await _nodeRecordProcessor.ProcessEntityNodesAsync(csv, headers, id, fileId);
        
        csv = _csvReaderService.CreateCsvReader(file);
        headers = _csvReaderService.ReadHeaders(csv, requiredHeaders);
    
        await _valueNodeProcessor.ProcessValueNodesAsync(csv, entityNodes, headers, id);
    }

}