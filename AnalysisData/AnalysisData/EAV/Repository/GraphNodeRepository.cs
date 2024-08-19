using AnalysisData.Data;
using AnalysisData.EAV.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class GraphNodeRepository : IGraphNodeRepository
{
    private readonly ApplicationDbContext _context;

    public GraphNodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ValueNode>> GetValueNodesByAttributeAsync(params string[] attributeNames)
    {
        return await _context.ValueNodes
            .Include(v => v.Entity)
            .Include(v => v.Attribute)
            .Where(v => attributeNames.Contains(v.Attribute.Name))
            .ToListAsync();
    }
}