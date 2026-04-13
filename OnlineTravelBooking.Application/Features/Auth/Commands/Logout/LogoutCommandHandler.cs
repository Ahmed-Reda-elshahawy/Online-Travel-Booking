using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Services;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.Logout;

internal class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IIdentityService _identityService;

    public LogoutCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Revoke the refresh token to prevent further access token generation
        // The access token will expire naturally on the client side (stateless JWT)
        var revoked = await _identityService.RevokeRefreshTokenAsync(request.RefreshToken);
        
        if (!revoked)
            return Result.Failure(new Error("TokenRevocationFailed", "Failed to revoke refresh token. It may have already been revoked or is invalid."));

        // Successfully logged out - client should clear both tokens locally
        return Result.Success();
    }
}
