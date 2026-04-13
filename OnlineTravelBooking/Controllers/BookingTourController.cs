using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.DTOs.TourDtos;
using OnlineTravelBooking.Application.Features.TourFeature.Commands;
using OnlineTravelBooking.Application.Features.TourFeature.Queries;
using OnlineTravelBooking.Controllers.Base;
using System.Security.Claims;

namespace OnlineTravelBooking.Controllers
{
    public class BookingTourController : ApiControllerBase
    {

        private readonly ILogger<BookingTourController> _logger;
        private readonly IMediator _mediatR;
        public BookingTourController(ILogger<BookingTourController> logger, IMediator mediatR)
        {
            _logger = logger;
            _mediatR = mediatR;
        }
        [HttpPost("AddBookingTour")]
        public async Task<IActionResult> AddBookingTour(BookingContDto bookingContDto)
        {
            var tourBookingDto = new TourBookingDto
            {
                NumberOfGuests = 2,
                TourScheduleId = 1,
                unitPrice = 150.00m,
            };
            var bookingDto = new BookingDto
            {
                UserId = 1,
                TourId = 1,
                Category = 2,
                Status = 0,
                TotalPrice = 20,
                Currency ="USD",
                BookingDate = DateTime.UtcNow,
                PaymentStatus = 0,
                PaymentMethod ="Cash",
            };
            //var tourBookingDto = new TourBookingDto
            //{
            //    NumberOfGuests = bookingContDto.NumberOfGuests,
            //    TourScheduleId = bookingContDto.TourScheduleId,
            //    unitPrice = bookingContDto.unitPrice,
            //};
            //var bookingDto = new BookingDto
            //{
            //    UserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
            //    TourId = bookingContDto.TourId,
            //    Category = bookingContDto.Category,
            //    Status = bookingContDto.Status,
            //    TotalPrice = bookingContDto.unitPrice*bookingContDto.NumberOfGuests,
            //    Currency = bookingContDto.Currency,
            //    BookingDate = DateTime.UtcNow,
            //    PaymentStatus = bookingContDto.PaymentStatus,
            //    PaymentMethod = bookingContDto.PaymentMethod,
            //};
            await _mediatR.Send(new AddBookingTour(tourBookingDto, bookingDto));
            return Ok();
        }

        [HttpPut("CancelBookingTour")]
        public async Task<IActionResult> CancelBookingTour()
        {
            var tourBookingDto = new TourBookingDto
            {
                Id = 1,
                NumberOfGuests = 2,
                TourScheduleId = 1,
                unitPrice = 150.00m,
            };
            var bookingDto = new BookingDto
            {
                Id=2,
                UserId = 1,
                TourId = 1,
                Category = 2,
                Status = 0,
                TotalPrice = 20,
                Currency = "USD",
                PaymentStatus = 0,
                PaymentMethod = "Cash",
                CancellationReason= "Change of plans",
                CancelledAt= DateTime.UtcNow,
            };
            //var tourBookingDto = new TourBookingDto
            //{
            //    Id= bookingContDto.Id ?? 0,
            //    NumberOfGuests = bookingContDto.NumberOfGuests,
            //    TourScheduleId = bookingContDto.TourScheduleId,
            //    unitPrice = bookingContDto.unitPrice,
            //};
            //var bookingDto = new BookingDto
            //{
            //    Id= bookingContDto.BookingId,
            //    UserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
            //    TourId = bookingContDto.TourId,
            //    Category = bookingContDto.Category,
            //    Status = bookingContDto.Status,
            //    TotalPrice = bookingContDto.unitPrice * bookingContDto.NumberOfGuests,
            //    Currency = bookingContDto.Currency,
            //    BookingDate = DateTime.UtcNow,
            //    PaymentStatus = bookingContDto.PaymentStatus,
            //    PaymentMethod = bookingContDto.PaymentMethod,
            //};
            await _mediatR.Send(new CancelBookingTour(tourBookingDto, bookingDto));
            return Ok("Cancelled Successfully");
        }
        [HttpGet("GetBookingTour/{bookingId}")]
        public async Task<IActionResult> GetBookingTour(int bookingId)
        {
            var result = await _mediatR.Send(new GetBookingTour(bookingId));
            return Ok(result);
        }

        }
}
