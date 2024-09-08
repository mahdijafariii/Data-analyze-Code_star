using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

namespace AnalysisData.Services.GraphService.Business.NodeManager;

public class NodeDataProcessor : INodeRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IValueNodeRepository _valueNodeRepository;
    private readonly int _batchSize;

    public NodeDataProcessor(IEntityNodeRepository entityNodeRepository, IValueNodeRepository valueNodeRepository, int batchSize = 100000)
    {
        _entityNodeRepository = entityNodeRepository;
        _valueNodeRepository = valueNodeRepository;
        _batchSize = batchSize;
    }

    public async Task ProcessNodesAsync(ICsvReaderProcessor csv, IEnumerable<AttributeNode> headers, string id, int fileId)
    {
        var entityNodes = new List<EntityNode>();
        var valueNodes = new List<ValueNode>();

        while (csv.Read())
        {
            var entityId = csv.GetField(id);
            if (string.IsNullOrEmpty(entityId)) continue;

            var entityNode = new EntityNode
            {
                Id = Guid.NewGuid(),
                Name = entityId,
                NodeFileReferenceId = fileId
            };
            entityNodes.Add(entityNode);

            foreach (var header in headers)
            {
                if (header.Name == id) continue;

                var valueNode = new ValueNode
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityNode.Id,
                    AttributeId = header.Id,
                    Value = csv.GetField(header.Name)
                };
                valueNodes.Add(valueNode);
            }
            
            if (entityNodes.Count >= _batchSize)
            {
                await BatchInsertAsync(entityNodes, valueNodes);
                entityNodes.Clear();
                valueNodes.Clear();
            }
        }
        
        if (entityNodes.Any())
        {
            await BatchInsertAsync(entityNodes, valueNodes);
        }
    }

    private async Task BatchInsertAsync(List<EntityNode> entityNodes, List<ValueNode> valueNodes)
    {
        await _entityNodeRepository.AddRangeAsync(entityNodes);
        await _valueNodeRepository.AddRangeAsync(valueNodes);
    }
}
