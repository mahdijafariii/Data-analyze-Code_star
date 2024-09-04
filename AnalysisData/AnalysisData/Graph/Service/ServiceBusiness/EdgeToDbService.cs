using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;


namespace AnalysisData.Graph.Service.ServiceBusiness;

public class EdgeToDbService : IEdgeToDbService
{
    private readonly IValueEdgeProcessor _valueEdgeProcessor;
    private readonly IEntityEdgeRecordProcessor _entityEdgeRecordProcessor;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderFactory _csvReaderFactory;
    private readonly ICsvHeaderValidator _csvHeaderValidator;
        
    public EdgeToDbService(IFromToProcessor fromToProcessor,
        ICsvReaderFactory csvReaderFactory, ICsvHeaderValidator csvHeaderValidator,
        IEntityEdgeRecordProcessor entityEdgeRecordProcessor, IValueEdgeProcessor valueEdgeProcessor)
    {
        _fromToProcessor = fromToProcessor;
        _csvReaderFactory = csvReaderFactory;
        _csvHeaderValidator = csvHeaderValidator;
        _entityEdgeRecordProcessor = entityEdgeRecordProcessor;
        _valueEdgeProcessor = valueEdgeProcessor;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string from, string to)
    {
        var csv = _csvReaderFactory.CreateCsvReader(file);
        var requiredHeaders = new List<string> { from, to };
        var headers = _csvHeaderValidator.ReadAndValidateHeaders(csv, requiredHeaders);
            
        await _fromToProcessor.ProcessFromToAsync(headers, from, to);
            
        var entityEdges = await _entityEdgeRecordProcessor.ProcessEntityEdgesAsync(csv, from, to);
        
        csv = _csvReaderFactory.CreateCsvReader(file);
        headers = _csvHeaderValidator.ReadAndValidateHeaders(csv, requiredHeaders);
        await _valueEdgeProcessor.ProcessEntityEdgeValuesAsync(csv, headers, from, to, entityEdges);
    }
}