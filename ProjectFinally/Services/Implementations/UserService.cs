using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Helpers;
using ProjectFinally.Models.DTOs.Users;
using ProjectFinally.Models.Entities;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Employee)
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive,
                RoleId = u.RoleId,
                RoleName = u.Role.RoleName,
                CreatedAt = u.CreatedAt,
                LastLoginAt = u.LastLoginAt,
                EmployeeId = u.Employee != null ? u.Employee.EmployeeId : null,
                EmployeeCode = u.Employee != null ? u.Employee.EmployeeCode : null
            })
            .ToListAsync();

        return users;
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Employee)
            .Where(u => u.UserId == userId)
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive,
                RoleId = u.RoleId,
                RoleName = u.Role.RoleName,
                CreatedAt = u.CreatedAt,
                LastLoginAt = u.LastLoginAt,
                EmployeeId = u.Employee != null ? u.Employee.EmployeeId : null,
                EmployeeCode = u.Employee != null ? u.Employee.EmployeeCode : null
            })
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if user already exists
        var userExists = await _context.Users
            .AnyAsync(u => u.Username == createUserDto.Username || u.Email == createUserDto.Email);

        if (userExists)
        {
            throw new InvalidOperationException("Username or email already exists");
        }

        // Get role to check if it's Employee
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == createUserDto.RoleId);
        if (role == null)
        {
            throw new InvalidOperationException("Invalid role ID");
        }

        // Create new user
        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PasswordHash = PasswordHasher.HashPassword(createUserDto.Password),
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            RoleId = createUserDto.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // If role is Employee, automatically create Employee record
        if (role.RoleName == "Employee")
        {
            // Generate employee code
            var employeeCount = await _context.Employees.CountAsync();
            var employeeCode = $"EMP{(employeeCount + 1).ToString("D3")}";

            var employee = new Employee
            {
                UserId = user.UserId,
                EmployeeCode = employeeCode,
                Department = "General",
                Position = "Employee",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Load employee for response
            await _context.Entry(user).Reference(u => u.Employee).LoadAsync();
        }

        // Load role for response
        await _context.Entry(user).Reference(u => u.Role).LoadAsync();

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            RoleId = user.RoleId,
            RoleName = user.Role.RoleName,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt,
            EmployeeId = user.Employee?.EmployeeId,
            EmployeeCode = user.Employee?.EmployeeCode
        };
    }

    public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        // Check if email is being changed and already exists for another user
        if (user.Email != updateUserDto.Email)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == updateUserDto.Email && u.UserId != userId);

            if (emailExists)
            {
                throw new InvalidOperationException("Email already exists");
            }
        }

        // Verify role exists
        var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == updateUserDto.RoleId);
        if (!roleExists)
        {
            throw new InvalidOperationException("Invalid role ID");
        }

        // Update user properties
        user.Email = updateUserDto.Email;
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.RoleId = updateUserDto.RoleId;
        user.UpdatedAt = DateTime.UtcNow;

        // Update password if provided
        if (!string.IsNullOrEmpty(updateUserDto.Password))
        {
            user.PasswordHash = PasswordHasher.HashPassword(updateUserDto.Password);
        }

        await _context.SaveChangesAsync();

        // Reload role if changed
        await _context.Entry(user).Reference(u => u.Role).LoadAsync();

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            RoleId = user.RoleId,
            RoleName = user.Role.RoleName,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _context.Roles
            .Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description
            })
            .ToListAsync();

        return roles;
    }
}
