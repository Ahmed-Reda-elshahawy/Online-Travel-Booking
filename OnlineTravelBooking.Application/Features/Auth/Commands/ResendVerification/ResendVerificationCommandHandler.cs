using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.ResendVerification;

public class ResendVerificationCommandHandler : IRequestHandler<ResendVerificationCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IIdentityService identityService;
    private readonly IEmailService emailService;

    public ResendVerificationCommandHandler(UserManager<ApplicationUser> userManager, IIdentityService identityService, IEmailService emailService)
    {
        this.userManager = userManager;
        this.identityService = identityService;
        this.emailService = emailService;
    }

    public async Task<Result<bool>> Handle(ResendVerificationCommand request, CancellationToken cancellationToken)
    {
        // get user by email and check if exists
        var user  = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Result.Failure<bool>(new Error("Validation", "User Not Found"));

        // if already verified, return error
        if (user.IsEmailVerified) return Result.Failure<bool>(new Error("Validation", "Email Already Verified"));

        // generate new OTP and update user
        var otp = identityService.GenerateOtp();
        user.EmailVerificationOtp = otp;
        user.EmailVerificationOtpExpiry = DateTime.UtcNow.AddMinutes(15);
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            return Result.Failure<bool>(new Error("Validation", $"Failed to update user: {errors}"));
        }

        // send email with new OTP
        await emailService.SendEmailVerificationOtpAsync(user.Email!, $"{user.FirstName} {user.LastName}", otp);

        return Result.Success(true);
    }
}
