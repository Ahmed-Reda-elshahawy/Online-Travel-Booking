using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.ResendVerification;

public record ResendVerificationCommand(string Email) : IRequest<Result<bool>>;
