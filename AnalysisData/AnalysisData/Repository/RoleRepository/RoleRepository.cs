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

    public async Task<Role> GetRole(int rolId)
    {
        return await _context.Roles.SingleOrDefaultAsync(x => x.Id == rolId);
    }

    public bool AddRole(Role role)
    {
        _context.Roles.Add(role);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteRole(int roleId)
    {
        var role = _context.Roles.FirstOrDefault(x => x.Id == roleId);
        if (role == null) return false;
        _context.Roles.Remove(role);
        _context.SaveChanges();
        return true;
    }
}