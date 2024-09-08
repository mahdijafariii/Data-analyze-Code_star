using AnalysisData.Data;
using AnalysisData.Dtos.GraphDto.EdgeDto;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository;

public class GraphEdgeRepository : IGraphEdgeRepository
{
    private readonly ApplicationDbContext _context;

    public GraphEdgeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EdgeInformationDto>> GetEdgeAttributeValues(Guid id)
    {
        var result = await _context.ValueEdges
            .Include(vn => vn.Entity)
            .Include(vn => vn.Attribute)
            .Where(vn => vn.Entity.Id == id)
            .Select(vn => new EdgeInformationDto
            {
                Attribute = vn.Attribute.Name,
                Value = vn.Value
            }).ToListAsync();
        return result;
    }

    public async Task<bool> IsEdgeAccessibleByUser(string userName, Guid edgeName)
    {
        var userNameGuid = Guid.Parse(userName);
        var entityIdSource = await _context.EntityEdges
            .Where(ee => ee.Id == edgeName)
            .Select(ee => ee.EntityIDSource)
            .FirstOrDefaultAsync();

        var uploadDataId = await _context.EntityNodes
            .Where(en => en.Id == entityIdSource)
            .Select(en => en.NodeFileReferenceId)
            .FirstOrDefaultAsync();

        var userFiles = await _context.UserFiles
            .Include(uf => uf.FileEntity)
            .Where(uf => uf.UserId == userNameGuid && uf.FileEntity.Id == uploadDataId)
            .ToListAsync();
        return userFiles.Count != 0;
    }
}