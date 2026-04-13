using Microsoft.AspNetCore.Http;
using OnlineTravelBooking.Application.Interfaces.Services;
using System.Security.Claims;

namespace OnlineTravelBooking.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public int UserId {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }

    public string Email
    {
        get
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}
