using ProjectFinally.Models.DTOs.Auth;

namespace ProjectFinally.Services.Interfaces;

public interface IOAuthService
{
    Task<LoginResponseDto> AuthenticateWithOAuth(OAuthLoginRequestDto request);
    Task<string?> VerifyGoogleToken(string token);
    Task<string?> VerifyFacebookToken(string token);
}
