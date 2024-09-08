using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager;

public class HeaderProcessor : IHeaderProcessor
{
    private readonly IAttributeNodeRepository _attributeNodeRepository;

    public HeaderProcessor(IAttributeNodeRepository attributeNodeRepository)
    {
        _attributeNodeRepository = attributeNodeRepository;
    }

    public async Task<IEnumerable<AttributeNode>> ProcessHeadersAsync(IEnumerable<string> headers, string uniqueAttribute)
    {
        var attributeNodes = new List<AttributeNode>();

        foreach (var header in headers.Where(h => h != uniqueAttribute))
        {
            var attributeNode = await _attributeNodeRepository.GetByNameAsync(header) 
                                ?? new AttributeNode { Name = header };
            attributeNodes.Add(attributeNode);
        }

        await _attributeNodeRepository.AddRangeAsync(attributeNodes);
        return attributeNodes;
    }
}
