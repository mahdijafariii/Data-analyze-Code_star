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

    public async Task<IEnumerable<UploadedFile>> GetFileUploadedInDb(int page, int limit)
    {
        return await _context.FileUploadedDb.Skip((page) * limit).Take(limit).ToListAsync();
    }
}