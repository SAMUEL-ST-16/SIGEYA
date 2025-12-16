using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Helpers;
using ProjectFinally.Models.DTOs.Auth;
using ProjectFinally.Models.Entities;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(ApplicationDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
    {
        // Find user with role
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Verify password
        if (!PasswordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive");
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Generate JWT token
        var token = _jwtHelper.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            RoleName = user.Role.RoleName,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
    {
        // Check if user already exists
        if (await UserExistsAsync(registerRequest.Username, registerRequest.Email))
        {
            throw new InvalidOperationException("Username or email already exists");
        }

        // Verify role exists
        var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == registerRequest.RoleId);
        if (!roleExists)
        {
            throw new InvalidOperationException("Invalid role ID");
        }

        // Create new user
        var user = new User
        {
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            PasswordHash = PasswordHasher.HashPassword(registerRequest.Password),
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            RoleId = registerRequest.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Load role for token generation
        await _context.Entry(user).Reference(u => u.Role).LoadAsync();

        // Generate JWT token
        var token = _jwtHelper.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            RoleName = user.Role.RoleName,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<bool> UserExistsAsync(string username, string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Username == username || u.Email == email);
    }
}
