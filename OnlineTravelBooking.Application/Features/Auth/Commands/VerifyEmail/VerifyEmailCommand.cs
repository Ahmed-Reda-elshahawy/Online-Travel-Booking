using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.VerifyEmail;

public record VerifyEmailCommand(string Email, string Otp) : IRequest<Result<UserDto>>;
