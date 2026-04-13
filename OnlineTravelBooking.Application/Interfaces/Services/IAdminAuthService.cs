using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Interfaces.Services;

public interface IAdminAuthService
{
    /// <summary>
    /// Validate credentials and sign in the admin user (cookie sign-in handled by implementation).
    /// </summary>
    Task<Result<UserDto>> LoginAsync(string email, string password, bool isPersistent = true);

    /// <summary>
    /// Sign out the current admin user.
    /// </summary>
    Task LogoutAsync();
}
