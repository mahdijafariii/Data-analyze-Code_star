using AnalysisData.EAV.Model;
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
    public DbSet<AttributeEdge> AttributeEdges { get; set; }
    public DbSet<AttributeNode> AttributeNodes { get; set; }
    public DbSet<EntityEdge> EntityEdges { get; set; }
    public DbSet<EntityNode> EntityNodes { get; set; }
    public DbSet<ValueEdge> ValueEdges { get; set; }
    public DbSet<ValueNode> ValueNodes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<FileEntity> FileUploadedDb { get; set; }
    public DbSet<UserFile> UserFiles { get; set; }
    public DbSet<Category> Categories { get; set; }
}