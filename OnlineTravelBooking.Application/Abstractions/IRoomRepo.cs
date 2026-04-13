
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Application.Abstractions;

public interface IRoomRepo 
{
	// 1️⃣ Add multiple rooms
	 Task AddRoomsAsync(List<Room> rooms);

	// 2️⃣ Get rooms by hotel
	Task<List<Room>> GetAllRoomsByHotelIdAsync(int hotelId);
	Task<Room?> GetRoomByIdAsync(int roomId);
	// Update
	Task UpdateRoomAsync(Room room);

	// Delete
	Task DeleteRoomAsync(Room room);
}
