using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Services;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<string>>
{
    private readonly IIdentityService identityService;
    public RefreshCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<Result<string>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var token = await identityService.RefreshTokenAsync(request.Token, request.RefreshToken);
        if (token == null)
            return Result.Failure<string>(new Error("Unauthorized", "Invalid token or refresh token"));

        return Result.Success(token);
    }
}
