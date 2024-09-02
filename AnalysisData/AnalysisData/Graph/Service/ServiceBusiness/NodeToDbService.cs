using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.EAV.Service.Business.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.EAV.Service;

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
        var csvReaderEnitityNodes = _csvReaderService.CreateCsvReader(file);
        var headers = _csvReaderService.ReadHeaders(csvReaderEnitityNodes);
        
        await _headerProcessor.ProcessHeadersAsync(headers, id);
        
        var entityNodes = await _nodeRecordProcessor.ProcessEntityNodesAsync(csvReaderEnitityNodes, headers, id, fileId);
        
        using var csvReaderForValueNodes = _csvReaderService.CreateCsvReader(file);
        var headers2 = _csvReaderService.ReadHeaders(csvReaderForValueNodes);
        await _valueNodeProcessor.ProcessValuesAsync(csvReaderForValueNodes, entityNodes, headers2, id);
    }
}