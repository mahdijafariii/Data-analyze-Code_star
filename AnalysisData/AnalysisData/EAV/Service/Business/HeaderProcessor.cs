using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.FileManage.Service.Business;

namespace AnalysisData.FileManage.Service;

public class HeaderProcessor : IHeaderProcessor
{
    private readonly IAttributeNodeRepository _attributeNodeRepository;

    public HeaderProcessor(IAttributeNodeRepository attributeNodeRepository)
    {
        _attributeNodeRepository = attributeNodeRepository;
    }
    
    public async Task ProcessHeadersAsync(string[] headers, string uniqueAttribute)
    {
        foreach (var header in headers)
        {
            if (header == uniqueAttribute) continue;

            var existingAttribute = await _attributeNodeRepository.GetByNameAttributeAsync(header);
            if (existingAttribute == null)
            {
                var attributeNode = new AttributeNode { Name = header };
                await _attributeNodeRepository.AddAsync(attributeNode);
            }
        }
    }
}