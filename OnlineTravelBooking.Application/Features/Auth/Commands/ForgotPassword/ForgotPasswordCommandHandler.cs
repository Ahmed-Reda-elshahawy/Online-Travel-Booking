using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IEmailService emailService;
    private readonly IIdentityService identityService;

    public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IEmailService emailService, IIdentityService identityService)
    {
        this.userManager = userManager;
        this.emailService = emailService;
        this.identityService = identityService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        // Check if the email exists in the system
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // For security reasons, we do not reveal that the email does not exist
            return Result.Success();
        }

        // Generate a 6-digit OTP
        var otp = identityService.GenerateOtp();

        // Set OTP and expiry (e.g., 15 minutes from now)
        user.PasswordResetOtp = otp;
        user.PasswordResetOtpExpiry = DateTime.UtcNow.AddMinutes(15);
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Result.Failure(new Error("OtpGenerationFailed", "Failed to generate OTP. Please try again."));
        }

        // Send OTP via email
        await emailService.SendPasswordResetOtpAsync(user.Email!, $"{user.FirstName} {user.LastName}", otp);

        return Result.Success();
    }
}