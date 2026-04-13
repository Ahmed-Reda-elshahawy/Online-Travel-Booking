using MediatR;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.RegisterAdmin;

public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, Result<UserDto>>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IIdentityService identityService;
    private readonly IEmailService emailService;

    public RegisterAdminCommandHandler(UserManager<ApplicationUser> userManager, IIdentityService identityService, IEmailService emailService)
    {
        this.userManager = userManager;
        this.identityService = identityService;
        this.emailService = emailService;
    }

    public async Task<Result<UserDto>> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        // Check if email is already taken
        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing != null)
            return Result.Failure<UserDto>(new Error("Validation", "Email is already taken"));

        // Create new user
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsEmailVerified = true,
            EmailConfirmed = true,
        };

        if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            var factory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            user.Location = factory.CreatePoint(new Coordinate(request.Longitude.Value, request.Latitude.Value));
        }

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errorMessage = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return Result.Failure<UserDto>(new Error("Validation", errorMessage));
        }

        // Assign Admin role
        await userManager.AddToRoleAsync(user, "Admin");

        var dto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, null, null);

        return Result.Success(dto);
    }
}
