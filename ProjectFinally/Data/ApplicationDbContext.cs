using Microsoft.EntityFrameworkCore;
using ProjectFinally.Models.Entities;

namespace ProjectFinally.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets - Phase 1
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Employee> Employees { get; set; }

    // DbSets - Phase 2
    public DbSet<YouTubeChannel> YouTubeChannels { get; set; }
    public DbSet<VideoCategory> VideoCategories { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<VideoAnalytics> VideoAnalytics { get; set; }
    public DbSet<ContentSchedule> ContentSchedules { get; set; }

    // DbSets - Phase 3
    public DbSet<AdSenseCampaign> AdSenseCampaigns { get; set; }
    public DbSet<AdRevenue> AdRevenues { get; set; }
    public DbSet<Models.Entities.Task> Tasks { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the Configurations folder
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
