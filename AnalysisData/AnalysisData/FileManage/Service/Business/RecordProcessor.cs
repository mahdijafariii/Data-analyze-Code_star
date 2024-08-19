using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using CsvHelper;

namespace AnalysisData.FileManage.Service.Business;

public class RecordProcessor : IRecordProcessor
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IAttributeNodeRepository _attributeNodeRepository;
    private readonly IValueNodeRepository _valueNodeRepository;

    public RecordProcessor(IEntityNodeRepository entityNodeRepository, IAttributeNodeRepository attributeNodeRepository, IValueNodeRepository valueNodeRepository)
    {
        _entityNodeRepository = entityNodeRepository;
        _attributeNodeRepository = attributeNodeRepository;
        _valueNodeRepository = valueNodeRepository;
    }

    public async Task ProcessRecordsAsync(CsvReader csv, string[] headers, string id)
    {
        while (csv.Read())
        {
            var entityId = csv.GetField(id);
            if (string.IsNullOrEmpty(entityId)) continue;

            var entityNode = await CreateEntityNodeAsync(entityId);
            await ProcessValuesAsync(csv, headers, id, entityNode);
        }
    }

    private async Task<EntityNode> CreateEntityNodeAsync(string entityId)
    {
        var entityNode = new EntityNode { Name = entityId };
        await _entityNodeRepository.AddAsync(entityNode);
        return entityNode;
    }

    private async Task ProcessValuesAsync(CsvReader csv, string[] headers, string id, EntityNode entityNode)
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
}