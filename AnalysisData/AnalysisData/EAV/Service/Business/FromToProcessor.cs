using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Service.Business.Abstraction;

namespace AnalysisData.EAV.Service.Business;

public class FromToProcessor : IFromToProcessor
{
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;

    public FromToProcessor(IAttributeEdgeRepository attributeEdgeRepository)
    {
        _attributeEdgeRepository = attributeEdgeRepository;
    }

    public async Task ProcessFromToAsync(IEnumerable<string> headers, string from,string to)
    {
        foreach (var header in headers)
        {
            if (header.Equals(from, StringComparison.OrdinalIgnoreCase) ||
                header.Equals(to, StringComparison.OrdinalIgnoreCase)) continue;

            var existingAttribute = await _attributeEdgeRepository.GetByNameAsync(header);
            if (existingAttribute == null)
            {
                var attributeEdge = new AttributeEdge { Name = header };
                await _attributeEdgeRepository.AddAsync(attributeEdge);
            }
        }
    }
}