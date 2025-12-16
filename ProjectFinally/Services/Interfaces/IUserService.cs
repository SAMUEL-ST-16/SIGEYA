using ProjectFinally.Models.DTOs.Users;

namespace ProjectFinally.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int userId);
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
}
