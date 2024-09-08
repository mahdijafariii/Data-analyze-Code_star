using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.EdgeManager;

public class EdgeDataProcessor : IEntityEdgeRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly IValueEdgeRepository _valueEdgeRepository;
    
    private readonly int _batchSize;

    public EdgeDataProcessor(
        IEntityNodeRepository entityNodeRepository, 
        IEntityEdgeRepository entityEdgeRepository, 
        IValueEdgeRepository valueEdgeRepository,
        int batchSize = 10000)
    {
        _entityNodeRepository = entityNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
        _valueEdgeRepository = valueEdgeRepository;
        _batchSize = batchSize;
    }

    public async Task ProcessEdgesAsync(ICsvReaderProcessor csv, IEnumerable<AttributeEdge> attributeEdges, string from, string to)
    {
        var entityEdges = new List<EntityEdge>();
        var valueEdges = new List<ValueEdge>();

        while (csv.Read())
        {
            var entityEdge = await CreateEntityEdgeAsync(csv, from, to);
            entityEdges.Add(entityEdge);

            foreach (var header in attributeEdges)
            {
                if (header.Name.Equals(from, StringComparison.OrdinalIgnoreCase) || 
                    header.Name.Equals(to, StringComparison.OrdinalIgnoreCase)) 
                    continue;
                
                var valueString = csv.GetField(header.Name);
                valueEdges.Add(new ValueEdge
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityEdge.Id,
                    AttributeId = header.Id,
                    Value = valueString
                });
            }

            if (entityEdges.Count >= _batchSize)
            {
                await BatchInsertAsync(entityEdges, valueEdges);
                entityEdges.Clear();
                valueEdges.Clear();
            }
        }
        
        if (entityEdges.Any())
        {
            await BatchInsertAsync(entityEdges, valueEdges);
        }
    }

    private async Task<EntityEdge> CreateEntityEdgeAsync(ICsvReaderProcessor csv, string from, string to)
    {
        var entityFrom = csv.GetField(from);
        var entityTo = csv.GetField(to);

        var fromNode = await _entityNodeRepository.GetByNameAsync(entityFrom);
        var toNode = await _entityNodeRepository.GetByNameAsync(entityTo);

        ValidateNodesExistence(fromNode, toNode, entityFrom, entityTo);

        return new EntityEdge
        {
            Id = Guid.NewGuid(),
            EntityIDSource = fromNode.Id,
            EntityIDTarget = toNode.Id
        };
    }

    private async Task BatchInsertAsync(List<EntityEdge> entityEdges, List<ValueEdge> valueEdges)
    {
        await _entityEdgeRepository.AddRangeAsync(entityEdges);
        await _valueEdgeRepository.AddRangeAsync(valueEdges);
    }

    private static void ValidateNodesExistence(EntityNode fromNode, EntityNode toNode, string entityFrom, string entityTo)
    {
        var missingNodeIds = new List<string>();

        if (fromNode == null) missingNodeIds.Add(entityFrom);
        if (toNode == null) missingNodeIds.Add(entityTo);

        if (missingNodeIds.Any())
        {
            throw new NodeNotFoundInEntityEdgeException(missingNodeIds);
        }
    }
}
