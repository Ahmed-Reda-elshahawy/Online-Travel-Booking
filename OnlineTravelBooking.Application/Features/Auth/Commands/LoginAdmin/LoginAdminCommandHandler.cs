using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.LoginAdmin;

internal class LoginAdminCommandHandler : IRequestHandler<LoginAdminCommand, Result<UserDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginAdminCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserDto>> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result.Failure<UserDto>(new Error("Unauthorized", "Invalid credentials"));

        // Verify password
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return Result.Failure<UserDto>(new Error("Unauthorized", "Invalid credentials"));

        // Ensure user is admin
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Admin"))
            return Result.Failure<UserDto>(new Error("Forbidden", "Access denied"));

        // Ensure email verified
        if (!user.IsEmailVerified)
            return Result.Failure<UserDto>(new Error("Validation", "Please verify your email before logging in"));

        // Return user dto without tokens (MVC will sign them in using cookie auth)
        var dto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, null, null);
        return Result.Success(dto);
    }
}
