using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result<UserDto>>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IIdentityService identityService;

    public VerifyEmailCommandHandler(UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        this.userManager = userManager;
        this.identityService = identityService;
    }

    public async Task<Result<UserDto>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        // get user by email and check if exists
        var user  = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Result.Failure<UserDto>(new Error("Validation", "User Not Found"));
        if (user.IsEmailVerified) return Result.Failure<UserDto>(new Error("Validation", "Email Already Verified"));

        // validate user otp
        if (string.IsNullOrEmpty(user.EmailVerificationOtp))
            return Result.Failure<UserDto>(new Error("Validation", "No verification OTP found. Please request a new one."));
        
        if (user.EmailVerificationOtp != request.Otp || user.EmailVerificationOtpExpiry < DateTime.UtcNow)
            return Result.Failure<UserDto>(new Error("Validation", "Invalid or Expired OTP"));

        // mark email as verified
        user.IsEmailVerified = true;
        user.EmailConfirmed = true;
        user.EmailVerificationOtp = null;
        user.EmailVerificationOtpExpiry = null;
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            return Result.Failure<UserDto>(new Error("Validation", $"Failed to update user: {errors}"));
        }

        // generate token after successful verification
        var (token, refreshToken) =  await identityService.GenerateTokenWithRefreshAsync(user);
        var userDto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email!, token, refreshToken);

        return Result.Success(userDto);
    }
}
