using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

namespace AnalysisData.Services.GraphService.Business.NodeManager;

public class ValueNodeProcessor : IValueNodeProcessor
{
    private readonly IAttributeNodeRepository _attributeNodeRepository;
    private readonly IValueNodeRepository _valueNodeRepository;
    private readonly int _batchSize;

    public ValueNodeProcessor(IAttributeNodeRepository attributeNodeRepository,
        IValueNodeRepository valueNodeRepository, int batchSize = 100000)
    {
        _attributeNodeRepository = attributeNodeRepository;
        _valueNodeRepository = valueNodeRepository;
        _batchSize = batchSize;
    }

    public async Task ProcessValueNodesAsync(ICsvReaderProcessor csv, IEnumerable<EntityNode> entityNodes, IEnumerable<string> headers, string id)
    {
        var attributeCache = new Dictionary<string, int>();
        var batch = new List<ValueNode>();

        while (csv.Read())
        {
            var uniqueId = csv.GetField(id);

            var entityNode = entityNodes.FirstOrDefault(e => e.Name == uniqueId);
            if (entityNode == null) continue;

            foreach (var header in headers)
            {
                if (header == id) continue;

                if (!attributeCache.TryGetValue(header, out var attributeId))
                {
                    var attribute = await _attributeNodeRepository.GetByNameAsync(header);
                    if (attribute == null) continue;

                    attributeId = attribute.Id;
                    attributeCache[header] = attributeId;
                }

                var valueNode = new ValueNode
                {
                    EntityId = entityNode.Id,
                    AttributeId = attributeId,
                    Value = csv.GetField(header)
                };

                batch.Add(valueNode);

                if (batch.Count >= _batchSize)
                {
                    await InsertBatchAsync(batch);
                    batch.Clear();
                }
            }
        }

        if (batch.Any())
        {
            await InsertBatchAsync(batch);
        }
    }

    private async Task InsertBatchAsync(IEnumerable<ValueNode> batch)
    {
        await _valueNodeRepository.AddRangeAsync(batch);
    }
}