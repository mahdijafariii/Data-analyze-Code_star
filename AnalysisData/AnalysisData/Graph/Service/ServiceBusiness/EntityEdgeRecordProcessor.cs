using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class EntityEdgeRecordProcessor : IEntityEdgeRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly int _batchSize;

    public EntityEdgeRecordProcessor(IEntityNodeRepository entityNodeRepository, IEntityEdgeRepository entityEdgeRepository, int batchSize = 1000)
    {
        _entityNodeRepository = entityNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;

        _batchSize = batchSize;
    }
    
    public async Task<IEnumerable<EntityEdge>> ProcessEntityEdgesAsync(CsvReader csv, string from, string to)
    {
        var entityEdges = new List<EntityEdge>();
        var batch = new List<EntityEdge>();
        while (csv.Read())
        {
            var entityFrom  = csv.GetField(from);
            var entityTo = csv.GetField(to);
            var fromNode = await _entityNodeRepository.GetByNameAsync(entityFrom);
            var toNode = await _entityNodeRepository.GetByNameAsync(entityTo);
            if (fromNode is null || toNode is null)
            {
                return null;
            }

            var entityEdge = new EntityEdge
                { EntityIDSource = fromNode.Id, EntityIDTarget = toNode.Id };
            
            entityEdges.Add(entityEdge);
            batch.Add(entityEdge);

            if (batch.Count >= _batchSize)
            {
                await InsertBatchAsync(batch);
                batch.Clear();
            }
            
        }
        if (batch.Any())
        {
            await InsertBatchAsync(batch);
        }
        return entityEdges;
    }
    private async Task InsertBatchAsync(IEnumerable<EntityEdge> batch)
    {
        await _entityEdgeRepository.AddRangeAsync(batch);
    }
}