using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.VerifyOtp;

public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result>
{
    private readonly UserManager<ApplicationUser> userManager;
    public VerifyOtpCommandHandler(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }


    public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        // Get the user
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result.Failure(new Error("InvalidOtp", "Invalid OTP or email."));

        // Verify the OTP
        if (string.IsNullOrEmpty(user.PasswordResetOtp) || user.PasswordResetOtp != request.Otp)
            return Result.Failure(new Error("InvalidOtp", "Invalid OTP."));

        // Verify The OTP expiry
        if (user.PasswordResetOtpExpiry == null || user.PasswordResetOtpExpiry < DateTime.UtcNow)
            return Result.Failure(new Error("OtpExpired", "The OTP has expired."));

        return Result.Success();
    }
}
