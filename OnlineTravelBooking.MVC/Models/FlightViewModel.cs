namespace OnlineTravelBooking.MVC.Models
{
    public class FlightViewModel
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string AirlineName { get; set; } = string.Empty;
        public DateTime DepartureTimeUtc { get; set; }
        public DateTime ArrivalTimeUtc { get; set; }
        public string OriginCity { get; set; } = string.Empty;
        public string OriginAirportCode { get; set; } = string.Empty;
        public string DestinationCity { get; set; } = string.Empty;
        public string DestinationAirportCode { get; set; } = string.Empty;
        public decimal EconomyPrice { get; set; }
        public decimal BusinessPrice { get; set; }
        public decimal FirstClassPrice { get; set; }
        public int TotalSeats { get; set; }
    }
}
