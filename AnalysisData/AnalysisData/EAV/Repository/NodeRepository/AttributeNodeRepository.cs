using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository.NodeRepository;

public class AttributeNodeRepository : IAttributeNodeRepository
{
    private readonly ApplicationDbContext _context;

    public AttributeNodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AttributeNode entity)
    {
        await _context.AttributeNodes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AttributeNode>> GetAllAsync()
    {
        return await _context.AttributeNodes.ToListAsync();
    }

    public async Task<AttributeNode> GetByIdAsync(int id)
    {
        return await _context.AttributeNodes.FindAsync(id);
    }

    public async Task<AttributeNode> GetByNameAttributeAsync(string name)
    {
        return await _context.AttributeNodes.FirstOrDefaultAsync(x => x.Name == name);
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _context.AttributeNodes.FindAsync(id);
        if (entity != null)
        {
            _context.AttributeNodes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<IEnumerable<string>> GetTypeOfCategory()
    {
        var result = await _context.ValueNodes
            .Where(v => v.Attribute.Name == "type")
            .Select(v => v.ValueString)
            .Distinct()
            .ToListAsync();
        return result;
    } 
}