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
        foreach (var header in headers)
        {
            if (header == uniqueAttribute) continue;

            var existingAttribute = await _attributeNodeRepository.GetByNameAsync(header);
            if (existingAttribute == null)
            {
                existingAttribute = new AttributeNode { Id = new Guid(), Name = header };
            }
            attributeNodes.Add(existingAttribute);
        }
        _attributeNodeRepository.AddRangeAsync(attributeNodes);
        return attributeNodes;
    }
}