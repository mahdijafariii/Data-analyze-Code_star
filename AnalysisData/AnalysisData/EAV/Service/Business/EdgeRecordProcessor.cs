using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.EAV.Service.Business.Abstraction;
using CsvHelper;

namespace AnalysisData.EAV.Service.Business;

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

    public async Task ProcessRecordsAsync(CsvReader csv, string[] headers, string from, string to)
    {
        while (csv.Read())
        {
            var fromId = csv.GetField(from);
            var toId = csv.GetField(to);
            if (string.IsNullOrEmpty(fromId) || string.IsNullOrEmpty(toId)) continue;

            var sourceEntityNode = (await _entityNodeRepository.GetAllAsync())
                .FirstOrDefault(a => a.Name == fromId);

            var targetEntityNode = (await _entityNodeRepository.GetAllAsync())
                .FirstOrDefault(a => a.Name == toId);
            var entityEdge = await CreateEntityEdgeAsync(sourceEntityNode, targetEntityNode);
            await ProcessValuesAsync(csv, headers, from, to, entityEdge);
        }
    }

    private async Task<EntityEdge> CreateEntityEdgeAsync(EntityNode sourceEntityNode, EntityNode targetEntityNode)
    {
        var entityEdge = new EntityEdge { EntityIDSource = sourceEntityNode.Id, EntityIDTarget = targetEntityNode.Id };
        await _entityEdgeRepository.AddAsync(entityEdge);
        return entityEdge;
    }

    private async Task ProcessValuesAsync(CsvReader csv, string[] headers, string from, string to,
        EntityEdge entityEdge)
    {
        foreach (var header in headers)
        {
            if (header.Equals(from, StringComparison.OrdinalIgnoreCase) ||
                header.Equals(to, StringComparison.OrdinalIgnoreCase)) continue;

            var attribute = _attributeEdgeRepository.GetByNameAttributeAsync(header);
            if (attribute == null) continue;

            var valueString = csv.GetField(header);
            var valueEdge = new ValueEdge
            {
                EntityId = entityEdge.Id,
                AttributeId = attribute.Id,
                ValueString = valueString
            };

            await _valueEdgeRepository.AddAsync(valueEdge);
        }
    }
}