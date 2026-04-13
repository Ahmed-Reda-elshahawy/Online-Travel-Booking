using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.LoginAdmin;

public record LoginAdminCommand(string Email, string Password) : IRequest<Result<UserDto>>;
