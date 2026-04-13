using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Application.Features.Search.FlightSearch.Query;
using OnlineTravelBooking.Application.Interfaces.Services;

namespace OnlineTravelBooking.Application.Features.Search.FlightSearch.Handler;

public class SearchFlightsQueryHandler : IRequestHandler<SearchFlightsQuery, Result<List<FlightDto>>>
{
    private readonly ISpecificationQueryExecutor _executor;

    public SearchFlightsQueryHandler(ISpecificationQueryExecutor executor)
    {
        _executor = executor;
    }

    public async Task<Result<List<FlightDto>>> Handle(SearchFlightsQuery request, CancellationToken cancellationToken)
    {
        int skip = (request.PageIndex - 1) * request.PageSize;

        var spec = new FlightSearchSpecification(
            request.UserLatitude,
            request.UserLongitude,
            request.DestinationCity,
            request.DepartureFromUtc,
            request.DepartureToUtc,
            request.ArrivalFromUtc,
            request.ArrivalToUtc,
            skip,
            request.PageSize,
            request.MaxDistanceInKm
        );

        var flights = await _executor.ListAsync(spec, cancellationToken);

        if (!flights.Any())
            return Result.Success(new List<FlightDto>());

        // Determine price per passenger based on requested cabin class
        var result = flights.Select(f =>
        {
            //decimal pricePerPassenger = request.CabinClass switch
            //{
            //    CabinClass.Business => f.BusinessPrice,
            //    CabinClass.First => f.FirstClassPrice,
            //    _ => f.EconomyPrice,
            //};

            //int pax = request.NumberOfPassengers ?? 1;
            //decimal totalPrice = pricePerPassenger * pax;

            return new FlightDto(
                f.Id,
                f.FlightNumber,
                f.AirlineName,
                f.DepartureTimeUtc,
                f.ArrivalTimeUtc,
                f.OriginCity,
                f.OriginAirportCode,
                f.DestinationCity,
                f.DestinationAirportCode,
                f.EconomyPrice,
                f.BusinessPrice,
                f.FirstClassPrice,
                f.TotalSeats
            );
        }).ToList();

        return Result.Success(result);
    }
}
