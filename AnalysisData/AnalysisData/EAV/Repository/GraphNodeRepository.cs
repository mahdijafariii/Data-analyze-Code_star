using System.Security.Claims;
using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class GraphNodeRepository : IGraphNodeRepository
{
    private readonly ApplicationDbContext _context;

    public GraphNodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EntityNode>> GetEntityNodesAsAdminAsync()
    {
        return await _context.EntityNodes.ToListAsync();
    }

    public async Task<IEnumerable<EntityNode>> GetEntityAsAdminNodesWithCategoryIdAsync(int categoryId)
    {
        var uploadDataIds = await _context.FileUploadedDb
            .Where(uploadData => uploadData.CategoryId == categoryId)
            .Select(uploadData => uploadData.Id)
            .ToListAsync();

        var result = await _context.EntityNodes
            .Where(entityNode => uploadDataIds.Contains(entityNode.UploadDataId))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetEntityNodesAsUserAsync(string userGuidId)
    {
        var guid = System.Guid.Parse(userGuidId);
        var fileIdQuery = await _context.UserFiles
            .Where(uf => uf.UserId.Equals(guid))
            .Select(uf => uf.FileId).ToListAsync();
        var result = await _context.EntityNodes
            .Where(en => fileIdQuery.Contains(en.UploadDataId))
            .ToListAsync();
        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetEntityAsUserNodesWithCategoryIdAsync(string userGuidId,
        int categoryId)
    {
        var guid = System.Guid.Parse(userGuidId);
        var fileIds = await _context.UserFiles
            .Where(uf => uf.UserId == guid)
            .Select(uf => uf.FileId)
            .ToListAsync();

        var result = await _context.EntityNodes
            .Include(en => en.UploadedFile)
            .Where(en => fileIds.Contains(en.UploadDataId) && en.UploadedFile.CategoryId == categoryId)
            .ToListAsync();
        return result;
    }


    public async Task<bool> IsNodeAccessibleByUser(string userName, string nodeName)
    {
        var guid = System.Guid.Parse(userName);
        var result = await _context.UserFiles
            .Include(uf => uf.UploadedFile)
            .ThenInclude(f => f.EntityNodes)
            .Where(uf => uf.UserId == guid)
            .SelectMany(uf => uf.UploadedFile.EntityNodes
                .Where(en => en.Name == nodeName)
                .Select(en => en.Name))
            .ToListAsync();
        if (result.Count==0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    public async Task<IEnumerable<dynamic>> GetNodeAttributeValue(string headerUniqueId)
    {
        var result = await _context.ValueNodes
            .Include(vn => vn.Entity)
            .Include(vn => vn.Attribute)
            .Where(vn => vn.Entity.Name == headerUniqueId)
            .Select(vn => new
            {
                Attribute = vn.Attribute.Name,
                Value = vn.ValueString
            }).ToListAsync();
        return result;
    }


    public async Task<IEnumerable<EntityNode>> GetNodeContainSearchInputAsAdmin(string input)
    {
        var result = await _context.EntityNodes
            .Where(a => a.Name.Contains(input))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputAsAdmin(string input)
    {
        var result = await _context.EntityNodes
            .Where(a => a.Name.StartsWith(input))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputAsAdmin(string input)
    {
        var result = await _context.EntityNodes
            .Where(a => a.Name.EndsWith(input))
            .ToListAsync();
        return result;
    }
    
    
    public async Task<IEnumerable<EntityNode>> GetNodeContainSearchInputAsUser(string username,string input)
    {
        var guidUserId = Guid.Parse(username);
        return await _context.UserFiles
            .Where(uf => uf.UserId == guidUserId)
            .Include(uf => uf.UploadedFile)
            .ThenInclude(uf => uf.EntityNodes)
            .SelectMany(uf => uf.UploadedFile.EntityNodes).Where(a => a.Name.Contains(input))
            .ToListAsync();
    }

    public async Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputAsUser(string username,string input)
    {
        var guidUserId = Guid.Parse(username);
        return await _context.UserFiles
            .Where(uf => uf.UserId == guidUserId)
            .Include(uf => uf.UploadedFile)
            .ThenInclude(uf => uf.EntityNodes)
            .SelectMany(uf => uf.UploadedFile.EntityNodes).Where(a => a.Name.StartsWith(input))
            .ToListAsync();
    }

    public async Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputAsUser(string username,string input)
    {
        var guidUserId = Guid.Parse(username);
        return await _context.UserFiles
            .Where(uf => uf.UserId == guidUserId)
            .Include(uf => uf.UploadedFile)
            .ThenInclude(uf => uf.EntityNodes)
            .SelectMany(uf => uf.UploadedFile.EntityNodes).Where(a => a.Name.EndsWith(input))
            .ToListAsync();
    }
}