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

    public async Task<IEnumerable<UploadedFile>> GetAllAsync()
    {
        return await _context.FileUploadedDb.ToListAsync();
    }

    public async Task<UploadedFile> GetByIdAsync(int id)
    {
        return await _context.FileUploadedDb.FindAsync(id);
    }

    public async Task<IEnumerable<UploadedFile>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Set<UploadedFile>()
            .Where(u => u.UploaderId == userId)
            .ToListAsync();
    }
    
    public async Task<int> GetNumberOfFileWithCategoryIdAsync(int categoryId)
    {
        return await _context.FileUploadedDb.CountAsync(x => x.CategoryId == categoryId);
    }

    public async Task AddAsync(UploadedFile uploadedFile)
    {
        await _context.FileUploadedDb.AddAsync(uploadedFile);
        await _context.SaveChangesAsync();
    }
}