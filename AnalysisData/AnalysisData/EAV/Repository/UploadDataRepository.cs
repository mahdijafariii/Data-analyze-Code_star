using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class UploadDataRepository : IUploadDataRepository
{
    private readonly ApplicationDbContext _context;

    public UploadDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UploadData>> GetAllAsync()
    {
        return await _context.UploadDatas.ToListAsync();
    }

    public async Task<UploadData> GetByIdAsync(int id)
    {
        return await _context.UploadDatas.FindAsync(id);
    }

    public async Task<IEnumerable<UploadData>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Set<UploadData>()
            .Where(u => u.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(UploadData uploadData)
    {
        await _context.UploadDatas.AddAsync(uploadData);
        await _context.SaveChangesAsync();
    }
}