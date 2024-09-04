
using AnalysisData.Exception;
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
    private readonly INodeValidator _nodeValidator;
    private readonly ICsvReader _csvReader;

    public EntityEdgeRecordProcessor(
        IEntityNodeRepository entityNodeRepository, 
        IEntityEdgeRepository entityEdgeRepository,
        INodeValidator nodeValidator,
        ICsvReader csvReader
    )
    {
        _entityNodeRepository = entityNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
        _nodeValidator = nodeValidator;
        _csvReader = csvReader;
    }

    public async Task<IEnumerable<EntityEdge>> ProcessEntityEdgesAsync(string from, string to)
    {
        var entityEdges = new List<EntityEdge>();
        var batch = new List<EntityEdge>();

        while (_csvReader.Read())
        {
            var entityEdge = await CreateEntityEdgeAsync(from, to);
            entityEdges.Add(entityEdge);
            batch.Add(entityEdge);
        }

        if (batch.Any())
        {
            await InsertBatchAsync(batch);
        }

        return entityEdges;
    }

    private async Task<EntityEdge> CreateEntityEdgeAsync(string from, string to)
    {
        var entityFrom = _csvReader.GetField(from);
        var entityTo = _csvReader.GetField(to);

        var fromNode = await _entityNodeRepository.GetByNameAsync(entityFrom);
        var toNode = await _entityNodeRepository.GetByNameAsync(entityTo);

        await _nodeValidator.ValidateNodesExistenceAsync(fromNode, toNode, entityFrom, entityTo);

        return new EntityEdge
        {
            EntityIDSource = fromNode.Id,
            EntityIDTarget = toNode.Id
        };
    }

    private async Task InsertBatchAsync(IEnumerable<EntityEdge> batch)
    {
        await _entityEdgeRepository.AddRangeAsync(batch);
    }
}