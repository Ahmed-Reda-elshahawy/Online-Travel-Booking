using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.Login;

internal class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<Result<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result.Failure<UserDto>(new Error("Unauthorized", "Invalid credentials"));

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return Result.Failure<UserDto>(new Error("Unauthorized", "Invalid credentials"));

        if (!user.IsEmailVerified)
            return Result.Failure<UserDto>(new Error("Validation", "Please verify your email before logging in"));

        var (token, refreshToken) = await _identityService.GenerateTokenWithRefreshAsync(user);
        var dto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, token, refreshToken);

        return Result.Success(dto);
    }
}
