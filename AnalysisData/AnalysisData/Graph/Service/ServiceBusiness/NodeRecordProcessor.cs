using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class NodeRecordProcessor : INodeRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IAttributeNodeRepository _attributeNodeRepository;
    private readonly IValueNodeRepository _valueNodeRepository;

    public NodeRecordProcessor(IEntityNodeRepository entityNodeRepository,
        IAttributeNodeRepository attributeNodeRepository, IValueNodeRepository valueNodeRepository)
    {
        _entityNodeRepository = entityNodeRepository;
        _attributeNodeRepository = attributeNodeRepository;
        _valueNodeRepository = valueNodeRepository;
    }

    public async Task ProcessRecordsAsync(CsvReader csv, IEnumerable<string> headers, string id, int fileId)
    {
        while (csv.Read())
        {
            var entityId = csv.GetField(id);
            if (string.IsNullOrEmpty(entityId)) continue;

            var entityNode = await CreateEntityNodeAsync(entityId, fileId);
            await ProcessValuesAsync(csv, headers, id, entityNode);
        }
    }

    private async Task<EntityNode> CreateEntityNodeAsync(string entityId, int fileId)
    {
        var entityNode = new EntityNode { Name = entityId, NodeFileReferenceId = fileId };
        await _entityNodeRepository.AddAsync(entityNode);
        return entityNode;
    }

    private async Task ProcessValuesAsync(CsvReader csv, IEnumerable<string> headers, string id, EntityNode entityNode)
    {
        foreach (var header in headers)
        {
            if (header == id) continue;

            var attribute = await _attributeNodeRepository.GetByNameAsync(header);
            if (attribute == null) continue;

            var valueString = csv.GetField(header);

            var valueNode = new ValueNode
            {
                EntityId = entityNode.Id,
                AttributeId = attribute.Id,
                Value = valueString
            };
            await _valueNodeRepository.AddAsync(valueNode);
        }
    }
}