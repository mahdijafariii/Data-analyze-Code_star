using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.EdgeManager;

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

    public async Task<IEnumerable<EntityEdge>> ProcessEntityEdgesAsync(ICsvReaderProcessor csv, string from, string to)
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

    private async Task<EntityEdge> CreateEntityEdgeAsync(ICsvReaderProcessor csv, string from, string to)
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
