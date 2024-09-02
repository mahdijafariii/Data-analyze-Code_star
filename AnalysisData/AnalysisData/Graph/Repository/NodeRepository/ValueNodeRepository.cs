using AnalysisData.Data;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Graph.Repository.NodeRepository;

public class ValueNodeRepository : IValueNodeRepository
{
    private readonly ApplicationDbContext _context;

    public ValueNodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ValueNode entity)
    {
        await _context.ValueNodes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ValueNode>> GetAllAsync()
    {
        return await _context.ValueNodes.ToListAsync();
    }

    public async Task<ValueNode> GetByIdAsync(int id)
    {
        return await _context.ValueNodes.FindAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.ValueNodes.FindAsync(id);
        if (entity != null)
        {
            _context.ValueNodes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}