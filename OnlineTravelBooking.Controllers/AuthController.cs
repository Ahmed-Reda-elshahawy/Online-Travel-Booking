using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Controllers.Base;
using OnlineTravelBooking.Application.Interfaces;

namespace OnlineTravelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}

public record RefreshRequest(string Token, string RefreshToken);
