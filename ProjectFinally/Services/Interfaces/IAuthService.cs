using ProjectFinally.Models.DTOs.Auth;

namespace ProjectFinally.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
    Task<bool> UserExistsAsync(string username, string email);
}
