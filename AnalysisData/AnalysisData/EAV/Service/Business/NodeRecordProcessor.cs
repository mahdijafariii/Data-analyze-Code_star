using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.EAV.Service.Business.Abstraction;
using CsvHelper;

namespace AnalysisData.EAV.Service.Business;

public class NodeRecordProcessor : INodeRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IAttributeNodeRepository _attributeNodeRepository;
    private readonly IValueNodeRepository _valueNodeRepository;

    public NodeRecordProcessor(IEntityNodeRepository entityNodeRepository, IAttributeNodeRepository attributeNodeRepository, IValueNodeRepository valueNodeRepository)
    {
        _entityNodeRepository = entityNodeRepository;
        _attributeNodeRepository = attributeNodeRepository;
        _valueNodeRepository = valueNodeRepository;
    }

    public async Task ProcessRecordsAsync(CsvReader csv, IEnumerable<string> headers, string id, string fileName)
    {
        var typeAttribute = await EnsureTypeAttributeExistsInDbAsync();

        while (csv.Read())
        {
            var entityId = csv.GetField(id);
            if (string.IsNullOrEmpty(entityId)) continue;

            var entityNode = await CreateEntityNodeAsync(entityId);
            await ProcessValuesAsync(csv, headers, id, entityNode);
            await AddFileNameAsValueNodeAsync(entityNode, typeAttribute, fileName);
        }
    }

    private async Task<AttributeNode> EnsureTypeAttributeExistsInDbAsync()
    {
        var typeAttribute = await _attributeNodeRepository.GetByNameAttributeAsync("type");
        
        if (typeAttribute == null)
        {
            typeAttribute = new AttributeNode { Name = "type" };
            await _attributeNodeRepository.AddAsync(typeAttribute);
        }
    
        return typeAttribute;
    }
    private async Task<EntityNode> CreateEntityNodeAsync(string entityId)
    {
        var entityNode = new EntityNode { Name = entityId };
        await _entityNodeRepository.AddAsync(entityNode);
        return entityNode;
    }

    private async Task ProcessValuesAsync(CsvReader csv, IEnumerable<string> headers, string id, EntityNode entityNode)
    {
        foreach (var header in headers)
        {
            if (header == id) continue;

            var attribute = await _attributeNodeRepository.GetByNameAttributeAsync(header);
            if (attribute == null) continue;

            var valueString = csv.GetField(header);

            var valueNode = new ValueNode
            {
                EntityId = entityNode.Id,
                AttributeId = attribute.Id,
                ValueString = valueString
            };
            await _valueNodeRepository.AddAsync(valueNode);
        }
    }

    private async Task AddFileNameAsValueNodeAsync(EntityNode entityNode, AttributeNode typeAttribute, string fileName)
    {
        var valueNode = new ValueNode
        {
            EntityId = entityNode.Id,
            AttributeId = typeAttribute.Id,
            ValueString = fileName
        };
        await _valueNodeRepository.AddAsync(valueNode);
    }
}
