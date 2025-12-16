using ProjectFinally.Helpers;
using ProjectFinally.Models.Entities;
using TaskEntity = ProjectFinally.Models.Entities.Task;

namespace ProjectFinally.Data.Seeders;

public static class DataSeeder
{
    public static async System.Threading.Tasks.Task SeedAsync(ApplicationDbContext context)
    {
        // Seed Roles
        if (!context.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role
                {
                    RoleName = "Admin",
                    Description = "Administrator with full system access",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleName = "Partner",
                    Description = "Business partner with permissions to manage all content except user deletion",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleName = "ContentManager",
                    Description = "Manages their own YouTube channels and videos only",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleName = "Employee",
                    Description = "Manages their own AdSense campaigns and tasks (cannot delete)",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleName = "Viewer",
                    Description = "Can view channels, videos, and campaigns (read-only access)",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // Seed Admin User
        if (!context.Users.Any())
        {
            var adminRole = context.Roles.First(r => r.RoleName == "Admin");

            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@youtubemanager.com",
                PasswordHash = PasswordHasher.HashPassword("Admin@123"),
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true,
                RoleId = adminRole.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            // Create Employee record for admin
            var adminEmployee = new Employee
            {
                EmployeeCode = "EMP001",
                Department = "Administration",
                Position = "System Administrator",
                PhoneNumber = "+1234567890",
                HireDate = DateTime.UtcNow,
                Salary = 0,
                IsActive = true,
                UserId = adminUser.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await context.Employees.AddAsync(adminEmployee);
            await context.SaveChangesAsync();
        }

        // Seed Video Categories
        if (!context.VideoCategories.Any())
        {
            var categories = new List<VideoCategory>
            {
                new VideoCategory { CategoryName = "Tutorial", Description = "How-to and educational videos", Color = "#FF5733", IsActive = true, CreatedAt = DateTime.UtcNow },
                new VideoCategory { CategoryName = "Gaming", Description = "Gaming content and walkthroughs", Color = "#33FF57", IsActive = true, CreatedAt = DateTime.UtcNow },
                new VideoCategory { CategoryName = "Review", Description = "Product and service reviews", Color = "#3357FF", IsActive = true, CreatedAt = DateTime.UtcNow },
                new VideoCategory { CategoryName = "Vlog", Description = "Personal vlogs and daily content", Color = "#FF33F5", IsActive = true, CreatedAt = DateTime.UtcNow },
                new VideoCategory { CategoryName = "News", Description = "News and updates", Color = "#F5FF33", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            await context.VideoCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
    }
}
