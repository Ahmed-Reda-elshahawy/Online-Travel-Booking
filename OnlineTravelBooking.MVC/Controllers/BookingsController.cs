using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Features.BaseBookings.Queries.GetAllBookings;
using OnlineTravelBooking.MVC.Models;

namespace OnlineTravelBooking.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly ISender _mediator;

        public BookingsController(ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var query = new GetAllBookingsQuery(pageIndex, pageSize);
                var paginatedBookings = await _mediator.Send(query);

                var bookingViewModels = paginatedBookings.Items.Select(b => new BookingItemViewModel
                {
                    Id = b.Id,
                    BookingReference = b.BookingReference,
                    Category = b.Category,
                    Status = b.Status,
                    TotalPrice = b.TotalPrice,
                    Currency = b.Currency,
                    BookingDate = b.BookingDate,
                    PaymentStatus = b.PaymentStatus,
                    PaymentMethod = b.PaymentMethod,
                    UserName = b.UserName,
                    UserEmail = b.UserEmail,
                    CancelledAt = b.CancelledAt,
                    CancellationReason = b.CancellationReason,
                    RefundAmount = b.RefundAmount
                }).ToList();

                var model = new BookingsViewModel
                {
                    Bookings = bookingViewModels,
                    PageIndex = paginatedBookings.PageNumber,
                    PageSize = paginatedBookings.PageSize,
                    TotalCount = paginatedBookings.TotalCount,
                    TotalPages = paginatedBookings.TotalPages,
                    HasPreviousPage = paginatedBookings.HasPreviousPage,
                    HasNextPage = paginatedBookings.HasNextPage
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading bookings: {ex.Message}";
                return View(new BookingsViewModel());
            }
        }
    }
}
