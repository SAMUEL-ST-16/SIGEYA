using Microsoft.AspNetCore.Mvc;
using ProjectFinally.Models.DTOs.Auth;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IOAuthService _oauthService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IOAuthService oauthService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _oauthService = oauthService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            var result = await _authService.LoginAsync(loginRequest);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Failed login attempt for username: {Username}", loginRequest.Username);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for username: {Username}", loginRequest.Username);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerRequest);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed registration attempt for username: {Username}", registerRequest.Username);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for username: {Username}", registerRequest.Username);
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }

    [HttpPost("oauth-login")]
    public async Task<IActionResult> OAuthLogin([FromBody] OAuthLoginRequestDto oauthRequest)
    {
        try
        {
            var result = await _oauthService.AuthenticateWithOAuth(oauthRequest);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Failed OAuth login attempt with provider: {Provider}", oauthRequest.Provider);
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid OAuth operation for provider: {Provider}", oauthRequest.Provider);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during OAuth login for provider: {Provider}", oauthRequest.Provider);
            return StatusCode(500, new { message = "An error occurred during OAuth login" });
        }
    }
}
