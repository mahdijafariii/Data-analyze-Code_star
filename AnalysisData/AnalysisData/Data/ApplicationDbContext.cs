using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    // public DbSet<SystemManager> SystemManagers { get; set; }
    // public DbSet<DataManager> DataManagers { get; set; }
    // public DbSet<DataAnalyst> DataAnalysts { get; set; }
}