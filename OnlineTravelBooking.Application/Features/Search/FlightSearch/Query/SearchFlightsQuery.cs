using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Domain.Common.Enums.Flights;

namespace OnlineTravelBooking.Application.Features.Search.FlightSearch.Query;

public record SearchFlightsQuery(
    double? UserLatitude,
    double? UserLongitude,
    string? DestinationCity,
    DateTime? DepartureFromUtc,
    DateTime? DepartureToUtc,
    DateTime? ArrivalFromUtc,
    DateTime? ArrivalToUtc,
    int? NumberOfPassengers,
    //CabinClass CabinClass = CabinClass.Economy,
    double? MaxDistanceInKm = 10,
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<Result<List<FlightDto>>>;
