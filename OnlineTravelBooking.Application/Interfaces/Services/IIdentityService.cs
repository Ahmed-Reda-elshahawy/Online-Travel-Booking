using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Interfaces.Services;

public interface IIdentityService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    Task<(string Token, string RefreshToken)> GenerateTokenWithRefreshAsync(ApplicationUser user);
    Task<string?> RefreshTokenAsync(string token, string refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    string GenerateOtp();
}
