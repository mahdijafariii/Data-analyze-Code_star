using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository.NodeRepository;

public class EntityNodeRepository : IEntityNodeRepository
{
    private readonly ApplicationDbContext _context;
    
    public EntityNodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EntityNode entity)
    {
        await _context.EntityNodes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<EntityNode>> GetAllAsync()
    {
        return await _context.EntityNodes.ToListAsync();
    }

    public async Task<EntityNode> GetByNameAsync(string id)
    {
        return await _context.EntityNodes.FirstOrDefaultAsync(x => x.Name == id);
    }

    public async Task<List<EntityNode>> GetNodesOfEdgeList(List<string> nodeIdes)
    {
        var entityNodes = new List<EntityNode>();
        foreach (var nodeId in nodeIdes)
        {
            var node = await GetByNameAsync(nodeId);
            if (nodeId != null)
            {
                entityNodes.Add(node);
            }
        }
        return entityNodes;
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _context.EntityNodes.FindAsync(id);
        if (entity != null)
        {
            _context.EntityNodes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    
}