using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository;

public class AttributeNodeRepositoryTests : IAttributeNodeRepository
{
    private readonly ApplicationDbContext _context;

    public AttributeNodeRepositoryTests(ApplicationDbContext context)
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

    public async Task<AttributeNode> GetByIdAsync(Guid id)
    {
        return await _context.AttributeNodes.FindAsync(id);
    }

    public async Task<AttributeNode> GetByNameAsync(string name)
    {
        return await _context.AttributeNodes.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task AddRangeAsync(IEnumerable<AttributeNode> attributeNodes)
    {
        {
            var existingIds = await _context.AttributeNodes
                .Where(an => attributeNodes.Select(n => n.Id).Contains(an.Id))
                .Select(an => an.Id)
                .ToListAsync();

            var newAttributeNodes = attributeNodes.Where(an => !existingIds.Contains(an.Id)).ToList();

            if (newAttributeNodes.Any())
            {
                await _context.AttributeNodes.AddRangeAsync(newAttributeNodes);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.AttributeNodes.FindAsync(id);
        if (entity != null)
        {
            _context.AttributeNodes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}