using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Features.Auth.Commands.ForgotPassword;
using OnlineTravelBooking.Application.Features.Auth.Commands.Login;
using OnlineTravelBooking.Application.Features.Auth.Commands.Logout;
using OnlineTravelBooking.Application.Features.Auth.Commands.Refresh;
using OnlineTravelBooking.Application.Features.Auth.Commands.Register;
using OnlineTravelBooking.Application.Features.Auth.Commands.ResendVerification;
using OnlineTravelBooking.Application.Features.Auth.Commands.ResetPassword;
using OnlineTravelBooking.Application.Features.Auth.Commands.VerifyEmail;
using OnlineTravelBooking.Application.Features.Auth.Commands.VerifyOtp;
using OnlineTravelBooking.Controllers.Base;

namespace OnlineTravelBooking.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
