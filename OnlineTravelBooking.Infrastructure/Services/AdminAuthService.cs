using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Infrastructure.Services;

public class AdminAuthService : IAdminAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AdminAuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<UserDto>> LoginAsync(string email, string password, bool isPersistent = true)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Failure<UserDto>(new Error("Unauthorized", "Invalid credentials"));

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            return Result.Failure<UserDto>(new Error("Unauthorized", "Invalid credentials"));

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Admin"))
            return Result.Failure<UserDto>(new Error("Forbidden", "Access denied"));

        if (!user.IsEmailVerified)
            return Result.Failure<UserDto>(new Error("Validation", "Please verify your email before logging in"));

        // Build UserDto
        var dto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, null, null);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = isPersistent,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await _signInManager.SignInAsync(user, authProperties);

        return Result.Success(dto);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    // Admin seeding helper
    public async Task EnsureAdminUserSeededAsync(string email, string password, string firstName = "Admin", string lastName = "User")
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
            return;

        var user = new ApplicationUser
        {
            UserName = email.Split('@')[0],
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true,
            IsEmailVerified = true
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            return; // optionally log

        // Ensure roles exist and assign Admin role
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Admin"))
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
