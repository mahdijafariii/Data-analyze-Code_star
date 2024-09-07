using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository;

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
    public async Task AddRangeAsync(IEnumerable<EntityNode> entityNodes)
    {
        await _context.EntityNodes.AddRangeAsync(entityNodes);
        await _context.SaveChangesAsync();
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