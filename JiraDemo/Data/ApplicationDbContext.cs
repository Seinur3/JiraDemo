using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Project => Set<Project>();
    public DbSet<ProjectMember> ProjectMember => Set<ProjectMember>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<Comment> Comment => Set<Comment>();
    public DbSet<Attachment> Attachment => Set<Attachment>();
    public DbSet<ActivityLog> ActivityLog => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<ProjectMember>().HasKey(u => new {u.ProjectId, u.UserId});
        base.OnModelCreating(modelBuilder);
       
    }
}