using MediatR;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Auth;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.Register;

internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService emailService;
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IEmailService emailService, IIdentityService identityService)
    {
        _userManager = userManager;
        this.emailService = emailService;
        _identityService = identityService;
    }

    public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if email is already taken
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing != null)
            return Result.Failure<UserDto>(new Error("Validation", "Email is already taken"));

        // Create new user with email verification OTP
        var otp = _identityService.GenerateOtp();
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsEmailVerified = false,
            EmailConfirmed = false,
            EmailVerificationOtp = otp,
            EmailVerificationOtpExpiry = DateTime.UtcNow.AddMinutes(15)
        };

        if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            var factory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            user.Location = factory.CreatePoint(new Coordinate(request.Longitude.Value, request.Latitude.Value));
        }

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errorMessage = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return Result.Failure<UserDto>(new Error("Validation", errorMessage));
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, "User");

        // send email verification OTP
        await emailService.SendEmailVerificationOtpAsync(user.Email, $"{user.FirstName} {user.LastName}", otp);

        var dto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, null, null);

        return Result.Success(dto);
    }
}
