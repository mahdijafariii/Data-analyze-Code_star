using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class UploadFileRepository : IUploadFileRepository
{
    private readonly ApplicationDbContext _context;

    public UploadFileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UploadData uploadData)
    {
        await _context.UploadDatas.AddAsync(uploadData);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UploadData>> GetAllAsync()
    {
        return await _context.UploadDatas.ToListAsync();
    }

    public async Task<UploadData> GetByIdAsync(int id)
    {
        return await _context.UploadDatas.FindAsync(id);
    }
    
    public async Task<UploadData> GetByNameAsync(string name)
    {
        return await _context.UploadDatas.FirstOrDefaultAsync(x => x.Name == name);
    }
    
    public async Task DeleteAsync(int id)
    {
        var entity = await _context.UploadDatas.FindAsync(id);
        if (entity != null)
        {
            _context.UploadDatas.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}