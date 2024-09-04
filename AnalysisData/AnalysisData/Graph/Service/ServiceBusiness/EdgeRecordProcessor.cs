using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class EdgeRecordProcessor : IEdgeRecordProcessor
{
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;
    private readonly IValueEdgeRepository _valueEdgeRepository;
    private readonly IEntityNodeRepository _entityNodeRepository;

    public EdgeRecordProcessor(IEntityEdgeRepository entityEdgeRepository,
        IAttributeEdgeRepository attributeEdgeRepository, IValueEdgeRepository valueEdgeRepository,
        IEntityNodeRepository entityNodeRepository)
    {
        _entityEdgeRepository = entityEdgeRepository;
        _attributeEdgeRepository = attributeEdgeRepository;
        _valueEdgeRepository = valueEdgeRepository;
        _entityNodeRepository = entityNodeRepository;
    }

    public async Task ProcessRecordsAsync(CsvReader csv, IEnumerable<string> headers, string from, string to)
    {
        while (await csv.ReadAsync())
        {
            var fromId = csv.GetField(from);
            var toId = csv.GetField(to);
            if (string.IsNullOrEmpty(fromId) || string.IsNullOrEmpty(toId)) continue;
        }
    }
}