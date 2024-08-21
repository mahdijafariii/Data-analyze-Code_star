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

    public IEnumerable<ValueNode> GetValueNodesAsync()
    {
        return _context.ValueNodes;
    }

    public async Task<IEnumerable<object>> GetAttributeValues(string headerUniqueId)
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