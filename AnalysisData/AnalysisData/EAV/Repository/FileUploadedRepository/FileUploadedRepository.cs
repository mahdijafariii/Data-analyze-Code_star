using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository.FileUploadedRepository;

public class FileUploadedRepository : IFileUploadedRepository
{
    private readonly ApplicationDbContext _context;

    public FileUploadedRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FileEntity>> GetUploadedFilesAsync(int page, int limit)
    {
        return await _context.FileUploadedDb.Include(x => x.Category).Skip((page) * limit).Take(limit).ToListAsync();
    }

    public async Task<IEnumerable<FileEntity>> GetAllAsync()
    {
        return await _context.FileUploadedDb.ToListAsync();
    }

    public async Task<FileEntity> GetByIdAsync(int id)
    {
        return await _context.FileUploadedDb.FindAsync(id);
    }

    public async Task<IEnumerable<FileEntity>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Set<FileEntity>()
            .Where(u => u.UploaderId == userId)
            .ToListAsync();
    }

    public async Task<int> GetNumberOfFileWithCategoryIdAsync(int categoryId)
    {
        return await _context.FileUploadedDb.CountAsync(x => x.CategoryId == categoryId);
    }

    public async Task AddAsync(FileEntity fileEntity)
    {
        await _context.FileUploadedDb.AddAsync(fileEntity);
        await _context.SaveChangesAsync();
    }
}