using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Application.Features.Flights.Commands.CreateFlight;
using OnlineTravelBooking.Application.Features.Flights.Commands.DeleteFlight;
using OnlineTravelBooking.Application.Features.Flights.Commands.UpdateFlight;
using OnlineTravelBooking.Application.Features.Flights.Queries.GetAllFlights;
using OnlineTravelBooking.Application.Features.Flights.Queries.GetFlightById;
using OnlineTravelBooking.Application.Features.Search.FlightSearch.Query;
using OnlineTravelBooking.MVC.Models;

namespace OnlineTravelBooking.MVC.Controllers;

[Authorize(Roles= "Admin")]
public class FlightsController : Controller
{
    private readonly ISender _mediator;

    public FlightsController(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllFlightsQuery());
        if (!result.IsSuccess)
            return View(result.Error.Message);

        var dtos = result.Value ?? new List<FlightDto>();
        var viewModels = dtos.Select(f => new FlightViewModel
        {
            Id = f.Id,
            FlightNumber = f.FlightNumber,
            AirlineName = f.AirlineName,
            DepartureTimeUtc = f.DepartureTimeUtc,
            ArrivalTimeUtc = f.ArrivalTimeUtc,
            OriginCity = f.OriginCity,
            OriginAirportCode = f.OriginAirportCode,
            DestinationCity = f.DestinationCity,
            DestinationAirportCode = f.DestinationAirportCode,
            EconomyPrice = f.EconomyPrice,
            BusinessPrice = f.BusinessPrice,
            FirstClassPrice = f.FirstClassPrice,
            TotalSeats = f.TotalSeats
        }).ToList();

        var model = new FlightsIndexViewModel
        {
            Flights = viewModels,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = viewModels.Count // We don't have total count from search handler; use current page count
        };

        return View(model);
    }

    public async Task<IActionResult> Search([FromQuery] SearchFlightsQuery request)
    {
        var result = await _mediator.Send(request);
        if (!result.IsSuccess)
            return View("Error");

        var dtos = result.Value ?? new List<FlightDto>();
        var viewModels = dtos.Select(f => new FlightViewModel
        {
            Id = f.Id,
            FlightNumber = f.FlightNumber,
            AirlineName = f.AirlineName,
            DepartureTimeUtc = f.DepartureTimeUtc,
            ArrivalTimeUtc = f.ArrivalTimeUtc,
            OriginCity = f.OriginCity,
            OriginAirportCode = f.OriginAirportCode,
            DestinationCity = f.DestinationCity,
            DestinationAirportCode = f.DestinationAirportCode,
            EconomyPrice = f.EconomyPrice,
            BusinessPrice = f.BusinessPrice,
            FirstClassPrice = f.FirstClassPrice,
            TotalSeats = f.TotalSeats
        }).ToList();

        var model = new FlightsIndexViewModel
        {
            Flights = viewModels,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = viewModels.Count
        };

        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFlightCommandViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var command = new CreateFlightCommand
        {
            FlightNumber = model.FlightNumber,
            AirlineName = model.AirlineName,
            AircraftType = model.AircraftType,
            OriginAirportCode = model.OriginAirportCode,
            OriginCity = model.OriginCity,
            DestinationAirportCode = model.DestinationAirportCode,
            DestinationCity = model.DestinationCity,
            DepartureTimeUtc = model.DepartureTimeUtc,
            ArrivalTimeUtc = model.ArrivalTimeUtc,
            FirstClassRows = model.FirstClassRows,
            BusinessClassRows = model.BusinessClassRows,
            EconomyClassRows = model.EconomyClassRows,
            Currency = model.Currency,
            EconomyPrice = model.EconomyPrice,
            BusinessPrice = model.BusinessPrice,
            FirstClassPrice = model.FirstClassPrice
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Error.Message);
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _mediator.Send(new GetFlightByIdQuery(id));
        if (!result.IsSuccess) return View("Error");

        var flight = result.Value;
        if (flight == null) return NotFound();

        var model = new UpdateFlightCommandViewModel
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            AirlineName = flight.AirlineName,
            AircraftType = flight.AircraftType,
            OriginAirportCode = flight.Route.Origin.Code,
            OriginCity = flight.Route.Origin.City,
            DestinationAirportCode = flight.Route.Destination.Code,
            DestinationCity = flight.Route.Destination.City,
            DepartureTimeUtc = flight.Schedule.DepartureTime,
            ArrivalTimeUtc = flight.Schedule.ArrivalTime,
            TotalSeats = flight.Capacity.TotalSeats,
            FirstClassRows = flight.Capacity.FirstClassRows,
            BusinessClassRows = flight.Capacity.BusinessClassRows,
            EconomyClassRows = flight.Capacity.EconomyClassRows,
            SeatsPerRow = flight.Capacity.SeatsPerRow,
            Currency = flight.Pricing.Economy.Currency,
            EconomyPrice = flight.Pricing.Economy.Price,
            BusinessPrice = flight.Pricing.Business.Price,
            FirstClassPrice = flight.Pricing.First.Price,
            Status = flight.Status
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateFlightCommandViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var command = new UpdateFlightCommand
        {
            Id = model.Id,
            FlightNumber = model.FlightNumber,
            AirlineName = model.AirlineName,
            AircraftType = model.AircraftType,
            OriginAirportCode = model.OriginAirportCode,
            OriginCity = model.OriginCity,
            DestinationAirportCode = model.DestinationAirportCode,
            DestinationCity = model.DestinationCity,
            DepartureTimeUtc = model.DepartureTimeUtc,
            ArrivalTimeUtc = model.ArrivalTimeUtc,
            TotalSeats = model.TotalSeats,
            FirstClassRows = model.FirstClassRows,
            BusinessClassRows = model.BusinessClassRows,
            EconomyClassRows = model.EconomyClassRows,
            SeatsPerRow = model.SeatsPerRow,
            Currency = model.Currency,
            EconomyPrice = model.EconomyPrice,
            BusinessPrice = model.BusinessPrice,
            FirstClassPrice = model.FirstClassPrice,
            Status = model.Status
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Error.Message);
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteFlightCommand(id));
        if (!result.IsSuccess)
        {
            // set an error message for display
            TempData["ErrorMessage"] = result.Error?.Message ?? "Failed to delete flight";
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Flight deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
