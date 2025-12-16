using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Helpers;
using ProjectFinally.Models.DTOs.Auth;
using ProjectFinally.Models.Entities;
using ProjectFinally.Services.Interfaces;
using System.Text.Json;

namespace ProjectFinally.Services.Implementations;

public class OAuthService : IOAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public OAuthService(
        ApplicationDbContext context,
        JwtHelper jwtHelper,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> AuthenticateWithOAuth(OAuthLoginRequestDto request)
    {
        string? email = null;
        string? name = null;
        string? provider = request.Provider.ToLower();

        // Verify token based on provider
        if (provider == "google")
        {
            var googleEmail = await VerifyGoogleToken(request.Token);
            if (googleEmail == null)
            {
                throw new UnauthorizedAccessException("Invalid Google token");
            }
            email = googleEmail;

            // Get Google user info
            var googlePayload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
            name = googlePayload.Name;
        }
        else if (provider == "facebook")
        {
            var facebookEmail = await VerifyFacebookToken(request.Token);
            if (facebookEmail == null)
            {
                throw new UnauthorizedAccessException("Invalid Facebook token");
            }
            email = facebookEmail;

            // Get Facebook user info (name will be extracted in VerifyFacebookToken)
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://graph.facebook.com/me?access_token={request.Token}&fields=name,email");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var fbData = JsonSerializer.Deserialize<JsonElement>(content);
                name = fbData.GetProperty("name").GetString();
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid OAuth provider. Only 'google' or 'facebook' are supported");
        }

        // Find or create user
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // Get Viewer role (default for OAuth registrations)
            var viewerRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == "Viewer");

            if (viewerRole == null)
            {
                throw new InvalidOperationException("Viewer role not found in database");
            }

            // Create new user
            // Generate username from email or use provided username
            string username = request.Username ?? email!.Split('@')[0];

            // Ensure username is unique
            int suffix = 1;
            string baseUsername = username;
            while (await _context.Users.AnyAsync(u => u.Username == username))
            {
                username = $"{baseUsername}{suffix}";
                suffix++;
            }

            // Split name into first and last name
            string[] nameParts = (name ?? email!.Split('@')[0]).Split(' ');
            string firstName = nameParts.Length > 0 ? nameParts[0] : "User";
            string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : provider == "google" ? "Google" : "Facebook";

            user = new User
            {
                Username = username,
                Email = email!,
                PasswordHash = PasswordHasher.HashPassword(Guid.NewGuid().ToString()), // Random password for OAuth users
                FirstName = firstName,
                LastName = lastName,
                RoleId = viewerRole.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Load role
            await _context.Entry(user).Reference(u => u.Role).LoadAsync();
        }
        else
        {
            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Check if user is active
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive");
        }

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

    public async Task<string?> VerifyGoogleToken(string token)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["OAuth:Google:ClientId"] ?? "" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            return payload.Email;
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> VerifyFacebookToken(string token)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Verify token with Facebook Graph API
            var appId = _configuration["OAuth:Facebook:AppId"];
            var appSecret = _configuration["OAuth:Facebook:AppSecret"];

            // Get app access token
            var appTokenResponse = await httpClient.GetAsync(
                $"https://graph.facebook.com/oauth/access_token?client_id={appId}&client_secret={appSecret}&grant_type=client_credentials");

            if (!appTokenResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var appTokenContent = await appTokenResponse.Content.ReadAsStringAsync();
            var appTokenData = JsonSerializer.Deserialize<JsonElement>(appTokenContent);
            var appAccessToken = appTokenData.GetProperty("access_token").GetString();

            // Verify user token
            var debugResponse = await httpClient.GetAsync(
                $"https://graph.facebook.com/debug_token?input_token={token}&access_token={appAccessToken}");

            if (!debugResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var debugContent = await debugResponse.Content.ReadAsStringAsync();
            var debugData = JsonSerializer.Deserialize<JsonElement>(debugContent);
            var data = debugData.GetProperty("data");

            if (!data.GetProperty("is_valid").GetBoolean())
            {
                return null;
            }

            // Get user email
            var userResponse = await httpClient.GetAsync(
                $"https://graph.facebook.com/me?access_token={token}&fields=email");

            if (!userResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var userContent = await userResponse.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<JsonElement>(userContent);

            return userData.GetProperty("email").GetString();
        }
        catch
        {
            return null;
        }
    }
}
