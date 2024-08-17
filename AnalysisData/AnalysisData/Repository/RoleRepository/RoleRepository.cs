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

    public async Task<Role> GetRoleById(int roleId)
    {
        return await _context.Roles.SingleOrDefaultAsync(x => x.Id == roleId);
    }

    public async Task<Role> GetRoleByName(string roleName)
    {
        return await _context.Roles.SingleOrDefaultAsync(x => x.RoleName == roleName);
    }

    public async Task<IReadOnlyList<Role>> GetAllRole()
    {
        return await _context.Roles.ToListAsync();
    }

    public bool AddRole(Role role)
    {
        _context.Roles.Add(role);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteRole(string roleName)
    {
        var role = _context.Roles.FirstOrDefault(x => x.RoleName == roleName);
        if (role == null) return false;
        _context.Roles.Remove(role);
        _context.SaveChanges();
        return true;
    }

    public void UpdateRole(Role role)
    {
        _context.Roles.Update(role);
        _context.SaveChanges();
    }
}