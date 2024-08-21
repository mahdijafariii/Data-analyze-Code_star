using AnalysisData.Data;
using AnalysisData.EAV.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class GraphEdgeRepository: IGraphEdgeRepository
{
    private readonly ApplicationDbContext _context;

    public GraphEdgeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<ValueEdge> GetValueEdgeAsync()
    {
        return _context.ValueEdges;
    }

    public IEnumerable<EntityEdge> GetEntityEdgeAsync()
    {
        return _context.EntityEdges;
    }

    public async Task<IEnumerable<dynamic>> GetEdgeAttributeValues(int id)
    {
        var result = await _context.ValueEdges
            .Include(vn => vn.Entity)    
            .Include(vn => vn.Attribute) 
            .Where(vn => vn.Entity.Id == id) 
            .Select(vn => new
            {
                Attribute = vn.Attribute.Name,
                Value = vn.ValueString
            }).ToListAsync();
        return result;
    }
}