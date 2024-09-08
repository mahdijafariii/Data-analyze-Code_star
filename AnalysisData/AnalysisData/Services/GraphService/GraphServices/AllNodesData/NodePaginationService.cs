using System.Security.Claims;
using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Exception.GraphException.CategoryException;
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using AnalysisData.Services.GraphService.CategoryService.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.AllNodesData.Abstraction;

namespace AnalysisData.Services.GraphService.GraphServices.AllNodesData;

public class NodePaginationService : INodePaginationService
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly ICategoryService _categoryService;


    public NodePaginationService(IGraphNodeRepository graphNodeRepository, ICategoryService categoryService)
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
                Id = g.Id,
                EntityName = g.Name,
            })
            .ToList();

        var count = groupedNodes.Count;
        var items = groupedNodes
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedNodeListDto(items, pageIndex, count, categoryName);
    }

    private async Task<IEnumerable<EntityNode>> GetEntityNodesForPaginationAsync(ClaimsPrincipal claimsPrincipal,
        int? category = null)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        var usernameGuid = Guid.Parse(username);
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
            valueNodes =
                await _graphNodeRepository.GetEntityNodeForUserWithCategoryIdAsync(usernameGuid, category.Value);
        }
        else if (category == null && role == "dataanalyst")
        {
            valueNodes = await _graphNodeRepository.GetEntityNodesForUserAsync(usernameGuid);
        }

        if (!valueNodes.Any() && category != null)
        {
            throw new CategoryResultNotFoundException();
        }

        return valueNodes;
    }
}