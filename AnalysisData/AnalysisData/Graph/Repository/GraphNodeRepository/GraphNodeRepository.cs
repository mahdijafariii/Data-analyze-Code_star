using AnalysisData.Data;
using AnalysisData.Graph.Model.Node;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Graph.Repository.GraphNodeRepository;

public class GraphNodeRepository : IGraphNodeRepository
{
    private readonly ApplicationDbContext _context;

    public GraphNodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EntityNode>> GetEntityNodesForAdminAsync()
    {
        return await _context.EntityNodes.ToListAsync();
    }

    public async Task<IEnumerable<EntityNode>> GetEntityNodesForAdminWithCategoryIdAsync(int categoryId)
    {
        var uploadDataIds = await _context.FileUploadedDb
            .Where(uploadData => uploadData.CategoryId == categoryId)
            .Select(uploadData => uploadData.Id)
            .ToListAsync();

        var result = await _context.EntityNodes
            .Where(entityNode => uploadDataIds.Contains(entityNode.NodeFileReferenceId))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetEntityNodesForUserAsync(Guid userGuidId)
    {
        var fileIdQuery = await _context.UserFiles
            .Where(uf => uf.UserId.Equals(userGuidId))
            .Select(uf => uf.FileId).ToListAsync();
        var result = await _context.EntityNodes
            .Where(en => fileIdQuery.Contains(en.NodeFileReferenceId))
            .ToListAsync();
        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetEntityNodeForUserWithCategoryIdAsync(Guid userGuidId,
        int categoryId)
    {
        var fileIds = await _context.UserFiles
            .Where(uf => uf.UserId == userGuidId)
            .Select(uf => uf.FileId)
            .ToListAsync();

        var result = await _context.EntityNodes
            .Include(en => en.FileEntity)
            .Where(en => fileIds.Contains(en.NodeFileReferenceId) && en.FileEntity.CategoryId == categoryId)
            .ToListAsync();
        return result;
    }


    public async Task<bool> IsNodeAccessibleByUser(Guid userName, int nodeId)
    {
        var result = await _context.UserFiles
            .Include(uf => uf.FileEntity)
            .ThenInclude(f => f.EntityNodes)
            .Where(uf => uf.UserId == userName)
            .SelectMany(uf => uf.FileEntity.EntityNodes
                .Where(en => en.Id == nodeId)
                .Select(en => en.Name))
            .ToListAsync();
        return result.Count != 0;
    }

    public async Task<IEnumerable<dynamic>> GetNodeAttributeValueAsync(int id)
    {
        var result = await _context.ValueNodes
            .Include(vn => vn.Entity)
            .Include(vn => vn.Attribute)
            .Where(vn => vn.Entity.Id == id)
            .Select(vn => new
            {
                Attribute = vn.Attribute.Name,
                Value = vn.Value
            }).ToListAsync();
        return result;
    }


    public async Task<IEnumerable<EntityNode>> GetNodeContainSearchInputForAdminAsync(string input)
    {
        var result = await _context.EntityNodes
            .Where(a => a.Name.Contains(input))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputForAdminAsync(string input)
    {
        var result = await _context.EntityNodes
            .Where(a => a.Name.StartsWith(input))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputForAdminAsync(string input)
    {
        var result = await _context.EntityNodes
            .Where(a => a.Name.EndsWith(input))
            .ToListAsync();
        return result;
    }


    public async Task<IEnumerable<EntityNode>> GetNodeContainSearchInputForUserAsync(Guid username, string input)
    {
        return await _context.UserFiles
            .Where(uf => uf.UserId == username)
            .Include(uf => uf.FileEntity)
            .ThenInclude(uf => uf.EntityNodes)
            .SelectMany(uf => uf.FileEntity.EntityNodes).Where(a => a.Name.Contains(input))
            .ToListAsync();
    }

    public async Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputForUserAsync(Guid username, string input)
    {
        return await _context.UserFiles
            .Where(uf => uf.UserId == username)
            .Include(uf => uf.FileEntity)
            .ThenInclude(uf => uf.EntityNodes)
            .SelectMany(uf => uf.FileEntity.EntityNodes).Where(a => a.Name.StartsWith(input))
            .ToListAsync();
    }

    public async Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputForUserAsync(Guid username, string input)
    {
        return await _context.UserFiles
            .Where(uf => uf.UserId == username)
            .Include(uf => uf.FileEntity)
            .ThenInclude(uf => uf.EntityNodes)
            .SelectMany(uf => uf.FileEntity.EntityNodes).Where(a => a.Name.EndsWith(input))
            .ToListAsync();
    }
}