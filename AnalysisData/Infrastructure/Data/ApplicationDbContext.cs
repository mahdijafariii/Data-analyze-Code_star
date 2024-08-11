using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    /*public ApplicationDbContext()
    {
    }*/
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<SystemManager> SystemManagers { get; set; }
    public DbSet<DataManager> DataManagers { get; set; }
    public DbSet<DataAnalyst> DataAnalysts { get; set; }
}