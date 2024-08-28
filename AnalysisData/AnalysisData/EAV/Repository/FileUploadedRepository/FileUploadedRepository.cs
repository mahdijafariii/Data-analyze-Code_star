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

    public async Task<int> GetTotalFilesCountAsync()
    {
        return await _context.FileUploadedDb.CountAsync();
    }

    public async Task<IEnumerable<UploadedFile>> GetFileUploadedInDb(int page, int limit)
    {
        return await _context.FileUploadedDb
            .Include(x => x.Category)
            .Skip(page * limit)
            .Take(limit)
            .ToListAsync();
    }
}