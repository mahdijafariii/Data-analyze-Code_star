using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
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

    public IEnumerable<ValueNode> GetValueNodesAsync()
    {
        return _context.ValueNodes;
    }

    public async Task<IEnumerable<dynamic>> GetNodeAttributeValue(string headerUniqueId)
    {
        var result = await _context.ValueNodes
            .Include(vn => vn.Entity)    
            .Include(vn => vn.Attribute) 
            .Where(vn => vn.Entity.Name == headerUniqueId) 
            .Select(vn => new
            {
                Attribute = vn.Attribute.Name,
                Value = vn.ValueString
            }).ToListAsync();
        return result;
    }
    
}