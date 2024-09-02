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

            var entityEdge = await CreateEntityEdgeAsync(fromId, toId);
            if (entityEdge is null)
            {
                continue;
            }

            await ProcessValuesAsync(csv, headers, from, to, entityEdge);
        }
    }

    private async Task<EntityEdge> CreateEntityEdgeAsync(string fromId, string toId)
    {
        var fromNode = await _entityNodeRepository.GetByNameAsync(fromId);
        var toNode = await _entityNodeRepository.GetByNameAsync(toId);
        if (fromNode is null || toNode is null)
        {
            return null;
        }

        var entityEdge = new EntityEdge
            { EntityIDSource = fromNode.Id, EntityIDTarget = toNode.Id };
        await _entityEdgeRepository.AddAsync(entityEdge);
        return entityEdge;
    }

    private async Task ProcessValuesAsync(CsvReader csv, IEnumerable<string> headers, string from, string to,
        EntityEdge entityEdge)
    {
        foreach (var header in headers)
        {
            if (header.Equals(from, StringComparison.OrdinalIgnoreCase) ||
                header.Equals(to, StringComparison.OrdinalIgnoreCase)) continue;

            var attribute = await _attributeEdgeRepository.GetByNameAsync(header);
            if (attribute == null) continue;

            var valueString = csv.GetField(header);
            var valueEdge = new ValueEdge
            {
                EntityId = entityEdge.Id,
                AttributeId = attribute.Id,
                Value = valueString
            };

            await _valueEdgeRepository.AddAsync(valueEdge);
        }
    }
}