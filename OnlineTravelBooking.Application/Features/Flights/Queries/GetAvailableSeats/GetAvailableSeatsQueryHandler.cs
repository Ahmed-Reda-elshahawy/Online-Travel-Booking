using MediatR;
using OnlineTravelBooking.Application.Common.Helpers.Flights;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Domain.Common.Enums.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetAvailableSeats;

public class GetAvailableSeatsQueryHandler : IRequestHandler<GetAvailableSeatsQuery, Result<AvailableSeatsDto>>
{
    private readonly IFlightRepository flightRepository;

    public GetAvailableSeatsQueryHandler(IFlightRepository flightRepository)
    {
        this.flightRepository = flightRepository;
    }

    public async Task<Result<AvailableSeatsDto>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
    {
        var flight = await flightRepository.GetByIdAsync(request.FlightId);
        if (flight == null)
            return Result.Failure<AvailableSeatsDto>(new Error("FlightNotFound", $"Flight with ID {request.FlightId} not found."));
        
        var bookedSeats = await flightRepository.GetBookedSeatsAsync(request.FlightId);
        var availableSeats = SeatHelper.GetAvailableSeatsWithClass(flight, bookedSeats);


        var dto = new AvailableSeatsDto
        {
            FlightId = flight.Id,
            TotalSeats = flight.TotalSeats,
            BookedSeats = bookedSeats.Count,
            AvailableSeats = availableSeats.Count,
            SeatsByClass =
            {
                Economy = availableSeats
                    .Where(s => s.CabinClass == CabinClass.Economy)
                    .Select(s => new SeatDto
                    {
                        SeatNumber = s.SeatNumber,
                        ExtraCharge = s.ExtraCharge
                    }).ToList(),
                Business = availableSeats
                    .Where(s => s.CabinClass == CabinClass.Business)
                    .Select(s => new SeatDto
                    {
                        SeatNumber = s.SeatNumber,
                        ExtraCharge = s.ExtraCharge
                    }).ToList(),
                First = availableSeats
                    .Where(s => s.CabinClass == CabinClass.First)
                    .Select(s => new SeatDto
                    {
                        SeatNumber = s.SeatNumber,
                        ExtraCharge = s.ExtraCharge
                    }).ToList()
            }
        };

        return Result.Success<AvailableSeatsDto>(dto);
    }
}