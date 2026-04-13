using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Result>;
