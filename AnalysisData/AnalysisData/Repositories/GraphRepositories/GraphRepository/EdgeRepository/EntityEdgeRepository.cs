using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository;

public class EntityEdgeRepository : IEntityEdgeRepository
{
    private readonly ApplicationDbContext _context;

    public EntityEdgeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EntityEdge entity)
    {
        await _context.EntityEdges.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<EntityEdge>> GetAllAsync()
    {
        return await _context.EntityEdges.ToListAsync();
    }
    public async Task AddRangeAsync(IEnumerable<EntityEdge> entityEdges)
    {
        var existingEntityIds = await _context.EntityNodes.Select(en => en.Id).ToListAsync();

        var validEntityEdges = entityEdges
            .Where(ee => existingEntityIds.Contains(ee.EntityIDSource))
            .ToList();

        if (validEntityEdges.Any())
        {
            await _context.EntityEdges.AddRangeAsync(validEntityEdges);
            await _context.SaveChangesAsync();
        }
        
    }
    public async Task<EntityEdge> GetByIdAsync(Guid id)
    {
        return await _context.EntityEdges.FindAsync(id);
    }

    public async Task<List<EntityEdge>> FindNodeLoopsAsync(Guid id)
    {
        return await _context.EntityEdges
            .Where(x => x.EntityIDSource == id || x.EntityIDTarget == id)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.EntityEdges.FindAsync(id);
        if (entity != null)
        {
            _context.EntityEdges.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}