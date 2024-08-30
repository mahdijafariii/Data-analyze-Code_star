﻿using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.EAV.Repository;

public class UserFileRepository : IUserFileRepository
{
    private readonly ApplicationDbContext _context;

    public UserFileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserFile userFile)
    {
        await _context.UserFiles.AddAsync(userFile);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserFile>> GetAllAsync()
    {
        return await _context.UserFiles.ToListAsync();
    }

    public async Task<UserFile> GetByUserIdAsync(string userId)
    {
        return await _context.UserFiles.FirstOrDefaultAsync(x => x.UserId.ToString() == userId);
    }

    public async Task<IEnumerable<UserFile>> GetByFileIdAsync(int fileId)
    {
        return await _context.Set<UserFile>().Include(x => x.User)
            .Where(u => u.FileId == fileId)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetUserIdsWithAccessToFileAsync(string fileId)
    {
        return await _context.Set<UserFile>()
            .Where(u => u.FileId.ToString() == fileId).Select(x => x.UserId.ToString())
            .ToListAsync();
    }

    public async Task DeleteByUserIdAsync(string userId)
    {
        var userFile = await _context.UserFiles.FirstOrDefaultAsync(x => x.UserId.ToString() == userId);
        if (userFile != null)
        {
            _context.UserFiles.Remove(userFile);
            await _context.SaveChangesAsync();
        }
    }
    
}