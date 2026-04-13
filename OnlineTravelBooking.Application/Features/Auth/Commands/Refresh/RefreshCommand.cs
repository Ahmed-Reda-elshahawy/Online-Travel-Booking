using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.Refresh;

public record RefreshCommand(string Token, string RefreshToken) : IRequest<Result<string>>;
