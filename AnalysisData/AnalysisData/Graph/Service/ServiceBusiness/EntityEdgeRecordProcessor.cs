using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class EntityEdgeRecordProcessor : IEntityEdgeRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly int _batchSize;

    public EntityEdgeRecordProcessor(
        IEntityNodeRepository entityNodeRepository, 
        IEntityEdgeRepository entityEdgeRepository 
        )
    {
        _entityNodeRepository = entityNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
    }

    public async Task<IEnumerable<EntityEdge>> ProcessEntityEdgesAsync(ICsvReader csv, string from, string to)
    {
        var entityEdges = new List<EntityEdge>();
        var batch = new List<EntityEdge>();

        while (csv.Read())
        {
            var entityEdge = await CreateEntityEdgeAsync(csv, from, to);
            entityEdges.Add(entityEdge);
            batch.Add(entityEdge);
        }

        if (batch.Any())
        {
            await InsertBatchAsync(batch);
        }

        return entityEdges;
    }

    private async Task<EntityEdge> CreateEntityEdgeAsync(ICsvReader csv, string from, string to)
    {
        var entityFrom = csv.GetField(from);
        var entityTo = csv.GetField(to);

        var fromNode = await _entityNodeRepository.GetByNameAsync(entityFrom);
        var toNode = await _entityNodeRepository.GetByNameAsync(entityTo);

        ValidateNodesExistence(fromNode, toNode, entityFrom, entityTo);

        return new EntityEdge
        {
            EntityIDSource = fromNode.Id,
            EntityIDTarget = toNode.Id
        };
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

    private async Task InsertBatchAsync(IEnumerable<EntityEdge> batch)
    {
        await _entityEdgeRepository.AddRangeAsync(batch);
    }
}
