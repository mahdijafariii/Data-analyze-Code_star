using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository;

public class ValueEdgeRepository : IValueEdgeRepository
{
    private readonly ApplicationDbContext _context;

    public ValueEdgeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ValueEdge entity)
    {
        await _context.ValueEdges.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ValueEdge>> GetAllAsync()
    {
        return await _context.ValueEdges.ToListAsync();
    }
    public async Task AddRangeAsync(IEnumerable<ValueEdge> valueEdges)
    {
        var existingEntityIds = await _context.EntityEdges.Select(ee => ee.Id).ToListAsync();

        var validValueEdges = valueEdges
            .Where(ve => existingEntityIds.Contains(ve.EntityId))
            .ToList();

        if (validValueEdges.Any())
        {
            await _context.ValueEdges.AddRangeAsync(validValueEdges);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<ValueEdge> GetByIdAsync(Guid id)
    {
        return await _context.ValueEdges.FindAsync(id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.ValueEdges.FindAsync(id);
        if (entity != null)
        {
            _context.ValueEdges.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}