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

    public async Task<IEnumerable<EntityNode>> GetEntityNodesAsync()
    {
        return await _context.EntityNodes.ToListAsync();
    }
    public async Task<IEnumerable<EntityNode>> GetEntityNodesWithCategoryAsync(string category)
    {
        var uploadDataIds = await _context.UploadDatas
            .Where(uploadData => uploadData.Category == category)
            .Select(uploadData => uploadData.Id)
            .ToListAsync();

        var result = await _context.EntityNodes
            .Where(entityNode => uploadDataIds.Contains(entityNode.UploadDataId))
            .ToListAsync();

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