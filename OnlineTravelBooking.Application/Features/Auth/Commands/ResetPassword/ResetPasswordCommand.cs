using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(string Email, string Otp, string NewPassword, string ConfirmPassword) : IRequest<Result>;