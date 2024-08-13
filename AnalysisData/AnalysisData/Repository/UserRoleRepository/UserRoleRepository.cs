using AnalysisData.Data;
using AnalysisData.Repository.UserRoleRepository.Abstraction;
using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.UserRoleRepository;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;

    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Add(UserRole userRole)
    {
        _context.UserRoles.Add(userRole);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteUserInUserRole(int userId)
    {
        var recordOfTable = _context.UserRoles.Where(x => x.UserId == userId);
        _context.UserRoles.RemoveRange(recordOfTable);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteRoleInUserRole(int roleId)
    {
        var recordOfTable = _context.UserRoles.Where(x => x.RoleId == roleId);
        _context.UserRoles.RemoveRange(recordOfTable);
        _context.SaveChanges();
        return true;
    }
}