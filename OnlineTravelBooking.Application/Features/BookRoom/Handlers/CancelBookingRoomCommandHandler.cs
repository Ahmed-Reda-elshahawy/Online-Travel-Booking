using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Features.BookRoom.Command;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;

public class CancelBookingRoomCommandHandler
	: IRequestHandler<CancelBookingRoomCommand, Result>
{
	private readonly IApplicationDbContext _context;

	public CancelBookingRoomCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(
		CancelBookingRoomCommand request,
		CancellationToken cancellationToken)
	{
		// ===============================
		// 1️- Load Booking
		// ===============================
		var booking = await _context.bookings
			.Include(b => b.BookingRoom)
			.FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

		if (booking == null)
			return Result.Failure(
				Error.NotFound("Booking.NotFound", "Booking not found"));

		// ===============================
		// 2️- Check Booking Status
		// ===============================
		if (booking.Status == BookingStatus.Cancelled)
			return Result.Failure(
				Error.Conflict("Booking.AlreadyCancelled", "Booking already cancelled"));

		if (booking.Status == BookingStatus.Confirmed)
			return Result.Failure(
				Error.Conflict("Booking.Completed", "Completed booking cannot be cancelled"));

		// ===============================
		// 3️- Find BookingRoom
		
		var bookingRoom = booking.BookingRoom
			.FirstOrDefault(br => br.Id == request.BookingRoomId);

		if (bookingRoom == null)
			return Result.Failure(
				Error.NotFound("BookingRoom.NotFound", "Booking room not found"));

		// ===============================
		// 4️- Remove BookingRoom
		
		booking.BookingRoom.Remove(bookingRoom);
		_context.bookingRooms.Remove(bookingRoom);

		// ===============================
		// 5️- Recalculate Total Price
		
		booking.TotalPrice -= bookingRoom.Total;

		// ===============================
		// 6️- Update Booking Status
		if (!booking.BookingRoom.Any())
		{
			booking.Status = BookingStatus.Cancelled;
		}

		// ===============================
		// 7️- Save
		
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
