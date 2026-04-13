using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;
using OnlineTravelBooking.Infrastructure.Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineTravelBooking.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly JwtSettings _settings;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(IOptions<JwtSettings> options, UserManager<ApplicationUser> userManager)
    {
        _settings = options.Value;
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim("firstName", user.FirstName ?? string.Empty),
            new Claim("lastName", user.LastName ?? string.Empty)
        };

        // Add role claims
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(string Token, string RefreshToken)> GenerateTokenWithRefreshAsync(ApplicationUser user)
    {
        var token = await GenerateTokenAsync(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        return (token, refreshToken);
    }

    public async Task<string?> RefreshTokenAsync(string token, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey))
            }, out var validatedToken);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return null;

            if (!int.TryParse(userIdClaim, out var userId)) return null;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiry == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
                return null;

            // Rotate refresh token for security
            var (newToken, newRefresh) = await GenerateTokenWithRefreshAsync(user);

            return newToken;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Revokes (invalidates) the refresh token for a user during logout.
    /// This prevents the client from obtaining new access tokens using the old refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to revoke</param>
    /// <returns>True if revocation was successful; False if token not found or already revoked</returns>
    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        // Find the user with the provided refresh token
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        
        if (user == null) 
            return false;

        // Clear both refresh token and expiry to effectively revoke it
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    /// <summary>
    /// Generates a cryptographically secure random refresh token.
    /// </summary>
    /// <returns>Base64-encoded refresh token string</returns>
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateOtp()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var randomNumber = BitConverter.ToUInt32(bytes, 0);
        return (randomNumber % 1000000).ToString("D6");
    }
}
