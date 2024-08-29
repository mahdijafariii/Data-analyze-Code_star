using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.Exception;

namespace AnalysisData.EAV.Service;

public class NodePaginationService : INodePaginationService
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly ICategoryService _categoryService;


    public NodePaginationService(IGraphNodeRepository graphNodeRepository, IGraphEdgeRepository graphEdgeRepository,
        IEntityNodeRepository entityNodeRepository, IEntityEdgeRepository entityEdgeRepository,
        ICategoryService categoryService)
    {
        _graphNodeRepository = graphNodeRepository;
        _categoryService = categoryService;
    }

    public async Task<PaginatedNodeListDto> GetAllNodesAsync(ClaimsPrincipal claimsPrincipal, int pageIndex,
        int pageSize, int? categoryId = null)
    {
        var valueNodes = await GetEntityNodesForPaginationAsync(claimsPrincipal, categoryId);

        string categoryName = null;
        if (categoryId.HasValue)
        {
            var category = await _categoryService.GetByIdAsync(categoryId.Value);
            categoryName = category?.Name;
        }

        var groupedNodes = valueNodes.Select(g => new PaginationNodeDto
            {
                EntityName = g.Name,
            })
            .ToList();

        var count = groupedNodes.Count;
        var items = groupedNodes
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(x => x.EntityName)
            .ToList();

        return new PaginatedNodeListDto(items, pageIndex, count, categoryName);
    }

    private async Task<IEnumerable<EntityNode>> GetEntityNodesForPaginationAsync(ClaimsPrincipal claimsPrincipal,
        int? category = null)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        IEnumerable<EntityNode> valueNodes = new List<EntityNode>();
        if (category == null && role != "dataanalyst")
        {
            valueNodes = await _graphNodeRepository.GetEntityNodesForAdminAsync();
        }
        else if (role != "dataanalyst" && category != null)
        {
            valueNodes = await _graphNodeRepository.GetEntityNodesForAdminWithCategoryIdAsync(category.Value);
        }
        else if (category != null && role == "dataanalyst")
        {
            valueNodes = await _graphNodeRepository.GetEntityNodeForUserWithCategoryIdAsync(username, category.Value);
        }
        else if (category == null && role == "dataanalyst")
        {
            valueNodes = await _graphNodeRepository.GetEntityNodesForUserAsync(username);
        }

        if (!valueNodes.Any() && category != null)
        {
            throw new CategoryResultNotFoundException();
        }

        if (valueNodes is null)
        {
            throw new NodeNotFoundException();
        }

        return valueNodes;
    }
}