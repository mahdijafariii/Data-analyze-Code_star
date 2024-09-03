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

    public async Task ProcessFromToAsync(IEnumerable<string> headers, string from, string to)
    {
        var attributeEdges = headers
            .Where(header => !header.Equals(from, StringComparison.OrdinalIgnoreCase) &&
                             !header.Equals(to, StringComparison.OrdinalIgnoreCase))
            .Select(header => new AttributeEdge { Name = header })
            .ToList();

        foreach (var attributeEdge in attributeEdges)
        {
            if (await _attributeEdgeRepository.GetByNameAsync(attributeEdge.Name) == null)
            {
                await _attributeEdgeRepository.AddAsync(attributeEdge);
            }
        }
    }
}