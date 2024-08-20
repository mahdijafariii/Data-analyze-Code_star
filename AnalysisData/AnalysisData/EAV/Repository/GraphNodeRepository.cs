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

    public IEnumerable<EntityNode> GetValueNodesAsync()
    {
        return _context.EntityNodes;
    }
}