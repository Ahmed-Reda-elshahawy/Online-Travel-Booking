
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Features.BookRoom.Command;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Common.Enums.HotelEnum;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.Application.Features.BookRoom.Handlers;

public class CreateHotelBookingCommandHandler
	: IRequestHandler<CreateHotelBookingCommand, Result<int>>
{
	private readonly IApplicationDbContext _context;

	public CreateHotelBookingCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<int>> Handle(
		CreateHotelBookingCommand request,
		CancellationToken cancellationToken)
	{
		// ===============================
		// 1️⃣ Basic Validation
		// ===============================
		if (request.Nights <= 0)
			return Result.Failure<int>(
				Error.Validation("Nights.Invalid", "Nights must be greater than zero"));

		if (request.Rooms == null || !request.Rooms.Any())
			return Result.Failure<int>(
				Error.Validation("Rooms.Empty", "At least one room must be selected"));

		// ===============================
		// 2️⃣ Check Hotel Exists
		// ===============================
		bool hotelExists = await _context.hotels
			.AnyAsync(h => h.Id == request.HotelId, cancellationToken);

		if (!hotelExists)
			return Result.Failure<int>(
				Error.NotFound("Hotel.NotFound", "Hotel not found"));

		// ===============================
		// 3️⃣ Create Booking (Aggregate Root)
		// ===============================
		var booking = new BaseBooking
        {
			UserId = request.UserId,
			HotelId = request.HotelId,
			BookingDate = DateTime.UtcNow,
			Status = BookingStatus.Pending
		};

		decimal totalPrice = 0;
		
		// ===============================
		// 4️⃣ Loop Rooms
		// ===============================
		foreach (var roomDto in request.Rooms)
		{
			// 4.1 Validate People
			if (roomDto.People <= 0)
				return Result.Failure<int>(
					Error.Validation("People.Invalid", "People count must be greater than zero"));

			// 4.2 Get Room
			var room = await _context.rooms
				.FirstOrDefaultAsync(r => r.Id == roomDto.RoomId, cancellationToken);

			if (room == null)
				return Result.Failure<int>(
					Error.NotFound("Room.NotFound", $"Room {roomDto.RoomId} not found"));

			// 4.3 Room belongs to hotel
			if (room.HotelId != request.HotelId)
				return Result.Failure<int>(
					Error.Validation("Room.InvalidHotel", "Room does not belong to selected hotel"));

			// 4.4 Capacity Check
			if (roomDto.People > room.MaxPeople)
				return Result.Failure<int>(
					Error.Validation("Room.CapacityExceeded", "Room capacity exceeded"));

			// 4.5 Availability Check (basic)
			
			

			if (room.availability != RoomAvailability.Available)
				return Result.Failure<int>(
					Error.Conflict("Room.NotAvailable", $"Room {room.Id} is not available"));

			// 4.6 Calculate Price (Snapshot)
			decimal roomTotalPrice = room.BasePricePerNight * request.Nights;

			// 4.7 Create BookingRoom
			var bookingRoom = new BookingRoom
			{
				RoomId = room.Id,
				CheckInDate = DateTime.UtcNow.Date,
				Nights = request.Nights,
				people = roomDto.People,
				Total = roomTotalPrice,
				Currency = room.Currency
			};

			_context.bookingRooms.Add(bookingRoom);
			totalPrice += roomTotalPrice;
		}

		// ===============================
		// 5️⃣ Finalize Booking
		// ===============================
		booking.TotalPrice = totalPrice;
		booking.Currency = "USD";

		// ===============================
		// 6️⃣ Save
		// ===============================
		_context.bookings.Add(booking);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success(booking.Id);
	}
}

