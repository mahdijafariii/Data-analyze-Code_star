using AnalysisData.Data;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Graph.Repository.NodeRepository;

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

    public async Task<EntityNode> GetByNameAsync(string name)
    {
        return await _context.EntityNodes.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<EntityNode> GetByIdAsync(int id)
    {
        return await _context.EntityNodes.FirstOrDefaultAsync(x => x.Id == id);
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