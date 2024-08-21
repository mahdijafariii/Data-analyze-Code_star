using AnalysisData.Data;
using AnalysisData.EAV.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class GraphNodeRepository : IGraphNodeRepository
{
    private readonly ApplicationDbContext _context;

    public GraphNodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<EntityNode> GetEntityNodesAsync()
    {
        return _context.EntityNodes;
    }
    public IEnumerable<EntityNode> GetEntityNodesWithCategoryAsync(string category)
    {
        var attributeId =  _context.AttributeNodes
            .Where(a => a.Name == "type")
            .Select(a => a.Id)
            .FirstOrDefault();

        var result = _context.ValueNodes
            .Include(v => v.Entity) 
            .Where(v => v.AttributeId == attributeId && v.ValueString == category)
            .Select(v => v.Entity)
            .ToList();
        return result;
    }
}