using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository;

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

    public async Task AddRangeAsync(IEnumerable<ValueNode> valueNodes)
    {
        await _context.ValueNodes.AddRangeAsync(valueNodes);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<ValueNode>> GetAllAsync()
    {
        return await _context.ValueNodes.ToListAsync();
    }

    public async Task<ValueNode> GetByIdAsync(Guid id)
    {
        return await _context.ValueNodes.FindAsync(id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.ValueNodes.FindAsync(id);
        if (entity != null)
        {
            _context.ValueNodes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}