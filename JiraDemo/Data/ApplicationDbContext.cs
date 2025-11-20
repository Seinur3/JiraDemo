using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Project { get; set; }
    public DbSet<ProjectMember> ProjectMember { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Attachment> Attachment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<ProjectMember>().HasKey(u => new {u.ProjectId, u.UserId});
        base.OnModelCreating(modelBuilder);
       
    }
}