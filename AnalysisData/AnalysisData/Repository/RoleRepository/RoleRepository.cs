using AnalysisData.Data;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repository.RoleRepository;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles.SingleOrDefaultAsync(x => x.Id == roleId);
    }

    public async Task<Role> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles.SingleOrDefaultAsync(x => x.RoleName == roleName);
    }

    public async Task<bool> AddRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRoleAsync(string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName);
        if (role == null) return false;
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    public async Task<List<Role>> GetAllRolesPaginationAsync(int page, int limit)
    {
        return await _context.Roles.Skip((page)*limit).Take(limit).ToListAsync();
    }
    
    public async Task<int> GetRolesCountAsync()
    {
        return await _context.Roles.CountAsync();
    }
}