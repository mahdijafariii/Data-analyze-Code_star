using AnalysisData.Data;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.RoleRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.User.Repository.RoleRepository;

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

    public async Task AddRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
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
        return await _context.Roles.Skip((page) * limit).Take(limit).ToListAsync();
    }

    public async Task<int> GetRolesCountAsync()
    {
        return await _context.Roles.CountAsync();
    }
    
    
    
    public async Task<IEnumerable<string>> GetRolesByPolicyAsync(string policy)
    {
        return await _context.Roles
            .Where(r => r.RolePolicy.ToLower() == policy.ToLower())
            .Select(r => r.RoleName.ToLower())
            .ToListAsync();
    }
}