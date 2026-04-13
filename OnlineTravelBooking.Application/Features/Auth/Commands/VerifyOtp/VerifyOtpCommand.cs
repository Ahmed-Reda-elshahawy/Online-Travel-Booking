using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.VerifyOtp;

public record VerifyOtpCommand(string Email, string Otp) : IRequest<Result>;
