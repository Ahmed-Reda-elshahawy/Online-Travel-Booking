using OnlineTravelBooking.Domain.Common.Enums.Flights;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Application.Common.Helpers.Flights;

public static class SeatHelper
{
    public static CabinClass GetCabinClassFromSeatNumber(string seatNumber, Flight flight)
    {
        // Extract row number from seat (e.g., "12A" → 12)
        var rowNumber = int.Parse(new string(seatNumber.TakeWhile(char.IsDigit).ToArray()));

        // Determine cabin class based on row ranges
        if (rowNumber <= flight.FirstClassRows)
            return CabinClass.First;

        if (rowNumber <= flight.FirstClassRows + flight.BusinessClassRows)
            return CabinClass.Business;

        return CabinClass.Economy;
    }

    public static List<string> GenerateAllSeats(Flight flight)
    {
        var seats = new List<string>();
        var totalRows = flight.FirstClassRows + flight.BusinessClassRows + flight.EconomyClassRows;
        var columns = GetColumns(flight.SeatsPerRow);

        for (int row = 1; row <= totalRows; row++)
        {
            foreach (var col in columns)
            {
                seats.Add($"{row}{col}");
            }
        }

        return seats;
    }

    public static List<AvailableSeat> GetAvailableSeatsWithClass(Flight flight, List<string> bookedSeats)
    {
        var allSeats = GenerateAllSeats(flight);
        var availableSeats = allSeats.Except(bookedSeats).ToList();

        return availableSeats.Select(seatNumber => new AvailableSeat
        {
            SeatNumber = seatNumber,
            CabinClass = GetCabinClassFromSeatNumber(seatNumber, flight),
            ExtraCharge = GetExtraCharge(seatNumber, flight)
        }).ToList();
    }

    public static decimal GetExtraCharge(string seatNumber, Flight flight)
    {
        var cabinClass = GetCabinClassFromSeatNumber(seatNumber, flight);
        var column = seatNumber.Last();

        // Extra charges for premium positions
        if (cabinClass == CabinClass.First)
            return 50m; // First class premium

        if (cabinClass == CabinClass.Business && (column == 'A' || column == 'F'))
            return 25m; // Business window seats

        if (column == 'A' || column == 'F')
            return 10m; // Economy window seats

        return 0m; // No extra charge
    }

    private static char[] GetColumns(int seatsPerRow)
    {
        return seatsPerRow switch
        {
            4 => new[] { 'A', 'B', 'C', 'D' },
            6 => new[] { 'A', 'B', 'C', 'D', 'E', 'F' },
            _ => new[] { 'A', 'B', 'C', 'D', 'E', 'F' }
        };
    }
}

public class AvailableSeat
{
    public string SeatNumber { get; set; } = string.Empty;
    public CabinClass CabinClass { get; set; }
    public decimal ExtraCharge { get; set; }
}