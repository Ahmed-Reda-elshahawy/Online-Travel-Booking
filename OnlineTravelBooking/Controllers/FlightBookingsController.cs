using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Application.Features.Flights.Commands.CreateFlightBooking;
using OnlineTravelBooking.Controllers.Base;

namespace OnlineTravelBooking.Controllers;

public class FlightBookingsController : ApiControllerBase
{
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateBooking([FromBody] CreateFlightBookingRequest request)
    {
        var command = new CreateFlightBookingCommand(request);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
