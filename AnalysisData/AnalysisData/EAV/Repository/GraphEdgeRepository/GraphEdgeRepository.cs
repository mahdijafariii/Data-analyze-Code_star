using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class GraphEdgeRepository: IGraphEdgeRepository
{
    private readonly ApplicationDbContext _context;

    public GraphEdgeRepository(ApplicationDbContext context)
    {
        _context = context;
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
                Value = vn.Value
            }).ToListAsync();
        return result;
    }
    
    public async Task<bool> IsEdgeAccessibleByUser(string userName, int edgeName)
    {
        var userNameGuid = Guid.Parse(userName);
        var entityIdSource = await _context.EntityEdges
            .Where(ee => ee.Id == edgeName)
            .Select(ee => ee.EntityIDSource)
            .FirstOrDefaultAsync();

        var uploadDataId = await _context.EntityNodes
            .Where(en => en.Id.ToString() == entityIdSource)
            .Select(en => en.NodeFileReferenceId)
            .FirstOrDefaultAsync();

        var userFiles = await _context.UserFiles
            .Include(uf => uf.FileEntity) 
            .Where(uf => uf.UserId == userNameGuid && uf.FileEntity.Id == uploadDataId)
            .ToListAsync();
        return userFiles.Count != 0;
    }
}