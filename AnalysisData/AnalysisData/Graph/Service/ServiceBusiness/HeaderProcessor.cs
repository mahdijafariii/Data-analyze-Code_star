using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class HeaderProcessor : IHeaderProcessor
{
    private readonly IAttributeNodeRepository _attributeNodeRepository;

    public HeaderProcessor(IAttributeNodeRepository attributeNodeRepository)
    {
        _attributeNodeRepository = attributeNodeRepository;
    }

    public async Task ProcessHeadersAsync(IEnumerable<string> headers, string uniqueAttribute)
    {
        foreach (var header in headers)
        {
            if (header == uniqueAttribute) continue;

            var existingAttribute = await _attributeNodeRepository.GetByNameAsync(header);
            if (existingAttribute == null)
            {
                var attributeNode = new AttributeNode { Name = header };
                await _attributeNodeRepository.AddAsync(attributeNode);
            }
        }
    }
}