using Microsoft.EntityFrameworkCore;
// using ProApi.Domain.Entities;

namespace ProApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // public DbSet<User> Users => Set<User>();
        // public DbSet<Project> Projects => Set<Project>();
        // public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        // public DbSet<TaskItem> Tasks => Set<TaskItem>();
        // public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
    }
}
