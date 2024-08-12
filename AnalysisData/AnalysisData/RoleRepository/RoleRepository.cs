using AnalysisData.Data;
using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.RoleRepository;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Role> GetRoles(int rolId)
    {
        return await _context.Roles.SingleOrDefaultAsync(x => x.Id == rolId);
    }
}