using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.MVC.Models;
using System.Diagnostics;
using MediatR;
using OnlineTravelBooking.Application.Features.Flights.Queries.GetAllFlights;
using OnlineTravelBooking.Application.Features.BaseBookings.Queries.GetAllBookings;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.Tour;
using Microsoft.AspNetCore.Identity;
using OnlineTravelBooking.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace OnlineTravelBooking.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISender _mediator;
        private readonly IGenericRepository<Tour> _tourRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ISender mediator, IGenericRepository<Tour> tourRepository, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _tourRepository = tourRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // If user is not authenticated, show login page first
            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                // Flights count via mediator
                int flightsCount = 0;
                try
                {
                    var flightsResult = await _mediator.Send(new GetAllFlightsQuery());
                    if (flightsResult?.IsSuccess == true && flightsResult.Value != null)
                        flightsCount = flightsResult.Value.Count;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to load flights for dashboard");
                }

                // Bookings count via mediator (paginated query provides TotalCount)
                int bookingsCount = 0;
                try
                {
                    var bookingsResult = await _mediator.Send(new GetAllBookingsQuery(1, 1));
                    bookingsCount = bookingsResult?.TotalCount ?? 0;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to load bookings for dashboard");
                }

                // Tours count via repository
                int toursCount = 0;
                try
                {
                    var tours = await _tourRepository.GetAll();
                    toursCount = tours?.Count() ?? 0;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to load tours for dashboard");
                }

                // Users count via UserManager
                int usersCount = 0;
                try
                {
                    usersCount = await _userManager.Users.CountAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to load users for dashboard");
                }

                ViewBag.FlightsCount = flightsCount;
                ViewBag.ToursCount = toursCount;
                ViewBag.BookingsCount = bookingsCount;
                ViewBag.UsersCount = usersCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard statistics");
                ViewBag.HasError = true;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
