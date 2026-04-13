using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> userManager;
    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }


    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // Get User by Email
        var user =  await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result.Failure(new Error("InvalidOtp", "Invalid OTP or email."));

        // Verify OTP
        if (string.IsNullOrEmpty(user.PasswordResetOtp) || user.PasswordResetOtp != request.Otp)
            return Result.Failure(new Error("InvalidOtp", "Invalid OTP or email."));

        // Verify OTP Expiry
        if (user.PasswordResetOtpExpiry == null || user.PasswordResetOtpExpiry < DateTime.UtcNow)
            return Result.Failure(new Error("OtpExpired", "The OTP has expired."));

        // Reset Password
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(new Error("PasswordResetFailed", errors));
        }

        // Clear OTP fields after successful password reset
        user.PasswordResetOtp = null;
        user.PasswordResetOtpExpiry = null;
        await userManager.UpdateAsync(user);

        return Result.Success();
    }
}
