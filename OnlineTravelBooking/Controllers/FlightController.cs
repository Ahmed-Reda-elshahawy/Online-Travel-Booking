using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Features.Flights.Queries.GetAvailableSeats;
using OnlineTravelBooking.Application.Features.Flights.Queries.GetFlightById;
using OnlineTravelBooking.Application.Features.Search.FlightSearch.Query;
using OnlineTravelBooking.Controllers.Base;

namespace OnlineTravelBooking.Controllers;

[Authorize(Roles = "User")]
public class FlightController : ApiControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        // Reuse SearchFlightsQuery with no filters to return all flights with pagination
        var query = new SearchFlightsQuery(
            UserLatitude: null,
            UserLongitude: null,
            DestinationCity: null,
            DepartureFromUtc: null,
            DepartureToUtc: null,
            ArrivalFromUtc: null,
            ArrivalToUtc: null,
            NumberOfPassengers: null,
            MaxDistanceInKm: 10,
            PageIndex: pageIndex,
            PageSize: pageSize
        );

        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchFlightsQuery request)
    {
        var result = await Mediator.Send(request);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlightById(int id)
    {
        var query = new GetFlightByIdQuery(id);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpGet("{flightId}/seats/available")]
    public async Task<IActionResult> GetAvailableSeats(int flightId)
    {
        var query = new GetAvailableSeatsQuery(flightId);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    //[HttpPost]
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> CreateFlight([FromBody] CreateFlightCommand command)
    //{
    //    var result = await Mediator.Send(command);
    //    return HandleResult(result);
    //}

    //[HttpPut]
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> UpdateFlight([FromBody] UpdateFlightCommand command)
    //{
    //    var result = await Mediator.Send(command);
    //    return HandleResult(result);
    //}

    //[HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> DeleteFlight(int id)
    //{
    //    var command = new DeleteFlightCommand(id);
    //    var result = await Mediator.Send(command);
    //    return HandleResult(result);
    //}
}
