using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class ValueEdgeProcessor
{
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;
    private readonly IValueEdgeRepository _valueEdgeRepository;
    private readonly int _batchSize;

    public ValueEdgeProcessor(IAttributeEdgeRepository attributeEdgeRepository, IValueEdgeRepository valueEdgeRepository, int batchSize)
    {
        _attributeEdgeRepository = attributeEdgeRepository;
        _valueEdgeRepository = valueEdgeRepository;
        _batchSize = batchSize;
    }
    
    public async Task ProcessValuesAsync(CsvReader csv, IEnumerable<string> headers, string from, string to, EntityEdge entityEdge)
    {
        foreach (var header in headers)
        {
            if (header.Equals(from, StringComparison.OrdinalIgnoreCase) ||
                header.Equals(to, StringComparison.OrdinalIgnoreCase)) continue;

            var attribute = await _attributeEdgeRepository.GetByNameAsync(header);
            if (attribute == null) continue;

            var valueString = csv.GetField(header);
            var valueEdge = new ValueEdge
            {
                EntityId = entityEdge.Id,
                AttributeId = attribute.Id,
                Value = valueString
            };

            await _valueEdgeRepository.AddAsync(valueEdge);
        }
    }
    
}