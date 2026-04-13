using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BaseBookingController : ControllerBase
{
	private readonly IBookingRepository _bookingRepository;

	public BaseBookingController(IBookingRepository bookingRepository)
	{
		_bookingRepository = bookingRepository;
	}

	// =====================================
	// 1️- Get All Bookings (Pagination)

	 	[HttpGet]
	public async Task<IActionResult> GetAllBookings(
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 10)
	{
		var result = await _bookingRepository
			.GetAllBookingsAsync(pageNumber, pageSize);

		return Ok(result);
	}

	// =====================================
	// 2️- Get Bookings By UserId

	[HttpGet("user/{userId}")]
	public async Task<IActionResult> GetBookingsByUserId(int userId)
	{
		var bookings = await _bookingRepository
			.GetBookingsByUserIdAsync(userId);

		if (!bookings.Any())
			return NotFound("No bookings found for this user");

		return Ok(bookings);
	}

	//// =====================================
	//// 3️- Create Booking

	//[HttpPost]
	//public async Task<IActionResult> CreateBooking([FromBody] BaseBooking booking)
	//{
	//	if (booking == null)
	//		return BadRequest("Invalid booking data");

	//	int bookingId = await _bookingRepository.AddBookingAsync(booking);

	//	return CreatedAtAction(
	//		nameof(GetBookingsByUserId),
	//		new { userId = booking.UserId },
	//		new { bookingId });
	//}

	// =====================================
	// 4️- Update Booking

	[HttpPut("{id}")]
	public IActionResult UpdateBooking(int id, [FromBody] BaseBooking booking)
	{
		if (id != booking.Id)
			return BadRequest("Booking ID mismatch");

		_bookingRepository.UpdateBookingAsync(booking);

		return NoContent();
	}
}
