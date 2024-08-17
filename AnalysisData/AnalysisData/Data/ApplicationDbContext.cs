using AnalysisData.Graph.DataManage.Model;
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
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}