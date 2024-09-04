using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class NodeToDbService : INodeToDbService
{
    private readonly ICsvReaderFactory _csvReaderFactory;
    private readonly ICsvHeaderValidator _csvHeaderValidator;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly INodeRecordProcessor _nodeRecordProcessor;
    private readonly IValueNodeProcessor _valueNodeProcessor;

    public NodeToDbService(
        ICsvReaderFactory csvReaderFactory, 
        ICsvHeaderValidator csvHeaderValidator, 
        IHeaderProcessor headerProcessor,
        INodeRecordProcessor nodeRecordProcessor, 
        IValueNodeProcessor valueNodeProcessor)
    {
        _csvReaderFactory = csvReaderFactory;
        _csvHeaderValidator = csvHeaderValidator;
        _headerProcessor = headerProcessor;
        _nodeRecordProcessor = nodeRecordProcessor;
        _valueNodeProcessor = valueNodeProcessor;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string id, int fileId)
    {
        var requiredHeaders = new List<string> { id };
        
        var csv = _csvReaderFactory.CreateCsvReader(file);
        var headers = _csvHeaderValidator.ReadAndValidateHeaders(csv, requiredHeaders);
        
        await _headerProcessor.ProcessHeadersAsync(headers, id);
        
        csv = _csvReaderFactory.CreateCsvReader(file);
        headers = _csvHeaderValidator.ReadAndValidateHeaders(csv, requiredHeaders);
        var entityNodes = await _nodeRecordProcessor.ProcessEntityNodesAsync(csv, headers, id, fileId);
        
        csv = _csvReaderFactory.CreateCsvReader(file);
        headers = _csvHeaderValidator.ReadAndValidateHeaders(csv, requiredHeaders);
        await _valueNodeProcessor.ProcessValueNodesAsync(csv, entityNodes, headers, id);
    }
}