using OnlineTravelBooking.MVC.Models;

namespace OnlineTravelBooking.MVC.Models;

public class FlightsIndexViewModel
{
    public List<FlightViewModel> Flights { get; set; } = new List<FlightViewModel>();
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => PageIndex > 1;
    public bool HasNext => PageIndex < TotalPages;
}
