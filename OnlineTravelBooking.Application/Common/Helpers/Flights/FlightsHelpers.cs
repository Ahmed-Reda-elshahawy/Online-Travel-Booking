using NetTopologySuite.Geometries;
using OnlineTravelBooking.Domain.Common.Enums.Flights;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Application.Common.Helpers.Flights;

public static class FlightsHelpers
{
    public static double? CalculateDistance(Point? origin, Point? destination)
    {
        if (origin == null || destination == null) return null;

        // Distance in meters, convert to kilometers
        return origin.Distance(destination) / 1000;
    }

    public static decimal GetPrice(Flight flight, CabinClass cabinClass) =>
        cabinClass switch
        {
            CabinClass.Economy => flight.EconomyPrice,
            CabinClass.Business => flight.BusinessPrice,
            CabinClass.First => flight.FirstClassPrice,
            _ => flight.EconomyPrice
        };

    public static string GenerateReference() =>
        $"BK{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
}
