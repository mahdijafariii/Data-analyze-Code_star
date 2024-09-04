﻿using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class EntityNodeRecordProcessor : INodeRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly int _batchSize;

    public EntityNodeRecordProcessor(IEntityNodeRepository entityNodeRepository, int batchSize = 100000)
    {
        _entityNodeRepository = entityNodeRepository;
        _batchSize = batchSize;
    }

    public async Task<IEnumerable<EntityNode>> ProcessEntityNodesAsync(CsvReader csv, IEnumerable<string> headers, string id, int fileId)
    {
        var entityNodes = new List<EntityNode>();
        var batch = new List<EntityNode>();

        while (csv.Read())
        {
            var entityId = csv.GetField(id);
            if (string.IsNullOrEmpty(entityId)) continue;

            var entityNode = new EntityNode { Name = entityId, NodeFileReferenceId = fileId };
            entityNodes.Add(entityNode);
            batch.Add(entityNode);

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

        return entityNodes;
    }

    private async Task InsertBatchAsync(IEnumerable<EntityNode> batch)
    {
        await _entityNodeRepository.AddRangeAsync(batch);
    }
    
}
