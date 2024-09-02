using AnalysisData.Data;
using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Graph.Repository.EdgeRepository;

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

    public async Task<EntityEdge> GetByIdAsync(int id)
    {
        return await _context.EntityEdges.FindAsync(id);
    }

    public async Task<List<EntityEdge>> FindNodeLoopsAsync(int id)
    {
        return await _context.EntityEdges
            .Where(x => x.EntityIDSource == id || x.EntityIDTarget == id)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.EntityEdges.FindAsync(id);
        if (entity != null)
        {
            _context.EntityEdges.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}