using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class UploadRepository : IUploadRepository
{
    private readonly ApplicationDbContext _context;

    public UploadRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Upload entity)
    {
        await _context.Uploads.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Upload>> GetAllAsync()
    {
        return await _context.Uploads.ToListAsync();
    }

    public async Task<Upload> GetByIdAsync(int id)
    {
        return await _context.Uploads.FindAsync(id);
    }
    
    public async Task<Upload> GetByContentAsync(string content)
    {
        return await _context.Uploads.SingleOrDefaultAsync(x => x.Content == content);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Uploads.FindAsync(id);
        if (entity != null)
        {
            _context.Uploads.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}