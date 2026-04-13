using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password, double? Latitude = null, double? Longitude = null) : IRequest<Result<UserDto>>;
