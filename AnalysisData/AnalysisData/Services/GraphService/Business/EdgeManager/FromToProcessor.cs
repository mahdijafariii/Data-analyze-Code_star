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

    public async Task<IEnumerable<AttributeEdge>> ProcessFromToAsync(IEnumerable<string> headers, string from, string to)
    {
        var attributeEdgesResult = new List<AttributeEdge>();
        
        var filteredHeaders = headers
            .Where(header => !header.Equals(from, StringComparison.OrdinalIgnoreCase) &&
                             !header.Equals(to, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (filteredHeaders.Count() != 0)
        {
            var existingAttributeEdges = await _attributeEdgeRepository.GetByNamesAsync(filteredHeaders);
            var existingEdgesDict = existingAttributeEdges
                .ToDictionary(ae => ae.Name, StringComparer.OrdinalIgnoreCase);
            var newAttributeEdges = new List<AttributeEdge>();

            foreach (var header in filteredHeaders)
            {
                if (existingEdgesDict.TryGetValue(header, out var existingAttributeEdge))
                {
                    attributeEdgesResult.Add(existingAttributeEdge);
                }
                else
                {
                    var newAttributeEdge = new AttributeEdge
                    {
                        Id = Guid.NewGuid(),
                        Name = header
                    };
                
                    newAttributeEdges.Add(newAttributeEdge);
                    attributeEdgesResult.Add(newAttributeEdge);
                }
            }
        
            if (newAttributeEdges.Any())
            {
                await _attributeEdgeRepository.AddRangeAsync(newAttributeEdges);
            }
        }
        return attributeEdgesResult;
    }
}