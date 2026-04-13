using OnlineTravelBooking.Domain.Entities.Flights;
using OnlineTravelBooking.Domain.Specifications;
using System.Linq.Expressions;

namespace OnlineTravelBooking.Application.Features.Search.FlightSearch;

public class FlightSearchSpecification : BaseSpecification<Flight>
{
    // Expose spatial parameters so infrastructure can apply DB spatial filters
    public double? UserLatitude { get; }
    public double? UserLongitude { get; }
    public double? MaxDistanceInKm { get; }

    public FlightSearchSpecification(
        double? userLatitude,
        double? userLongitude,
        string? destinationCity,
        DateTime? departureFromUtc,
        DateTime? departureToUtc,
        DateTime? arrivalFromUtc,
        DateTime? arrivalToUtc,
        int skip,
        int take,
        double? maxDistanceInKm = null)
    {
        UserLatitude = userLatitude;
        UserLongitude = userLongitude;
        MaxDistanceInKm = maxDistanceInKm;

        // Build criteria
        Expression<Func<Flight, bool>> criteria = f => true;

        if (!string.IsNullOrWhiteSpace(destinationCity))
        {
            var destLower = destinationCity.Trim().ToLower();
            criteria = Combine(criteria, f => f.DestinationCity.ToLower() == destLower);
        }

        if (departureFromUtc.HasValue)
        {
            criteria = Combine(criteria, f => f.DepartureTimeUtc >= departureFromUtc.Value);
        }

        if (departureToUtc.HasValue)
        {
            criteria = Combine(criteria, f => f.DepartureTimeUtc <= departureToUtc.Value);
        }

        if (arrivalFromUtc.HasValue)
        {
            criteria = Combine(criteria, f => f.ArrivalTimeUtc >= arrivalFromUtc.Value);
        }

        if (arrivalToUtc.HasValue)
        {
            criteria = Combine(criteria, f => f.ArrivalTimeUtc <= arrivalToUtc.Value);
        }

        Criterea = criteria;

        // Includes (if needed)
        ApplyPaging(skip, take);
        ApplyOrderBy(f => f.DepartureTimeUtc);
    }

    // Helper to combine expressions with AND
    private static Expression<Func<Flight, bool>> Combine(Expression<Func<Flight, bool>> first, Expression<Func<Flight, bool>> second)
    {
        var param = Expression.Parameter(typeof(Flight));

        var left = Expression.Invoke(first, param);
        var right = Expression.Invoke(second, param);
        var body = Expression.AndAlso(left, right);

        return Expression.Lambda<Func<Flight, bool>>(body, param);
    }
}
