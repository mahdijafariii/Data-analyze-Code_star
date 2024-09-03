using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.EAV.Service.Business.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

namespace AnalysisData.EAV.Service;

public class EdgeToDbService : IEdgeToDbService
{
    private readonly IValueEdgeProcessor _valueEdgeProcessor;
    private readonly IEntityEdgeRecordProcessor _entityEdgeRecordProcessor;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderService _csvReaderService;
    
    public EdgeToDbService(IFromToProcessor fromToProcessor,
        ICsvReaderService csvReaderService, IEntityEdgeRecordProcessor entityEdgeRecordProcessor, IValueEdgeProcessor valueEdgeProcessor)
    {
        _fromToProcessor = fromToProcessor;
        _csvReaderService = csvReaderService;
        _entityEdgeRecordProcessor = entityEdgeRecordProcessor;
        _valueEdgeProcessor = valueEdgeProcessor;
    }

    public async Task ProcessCsvFileAsync(IFormFile file, string from, string to)
    {
        var csv = _csvReaderService.CreateCsvReader(file);
        var headers = _csvReaderService.ReadHeaders(csv);

        await _fromToProcessor.ProcessFromToAsync(headers, from, to);
        var entityEdges = await _entityEdgeRecordProcessor.ProcessEntityEdgesAsync(csv, from, to);

        csv = _csvReaderService.CreateCsvReader(file);
        headers = _csvReaderService.ReadHeaders(csv);

        await _valueEdgeProcessor.ProcessEntityEdgeValuesAsync(csv, headers, from, to, entityEdges);
    }
}