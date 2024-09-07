using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.EdgeManager;

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