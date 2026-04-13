using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<Result>;