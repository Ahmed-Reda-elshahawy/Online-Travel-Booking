
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Application.Abstractions;

public interface IHotelRepo
{
	// 1️⃣ Add a new hotel
	Task AddHotelAsync(Hotel hotel);

	// 2️⃣ Get all hotels with pagination
	Task<PaginatedList<Hotel>> GetAllHotelsAsync(int pageNumber, int pageSize);

	// 3️⃣ Get hotel by ID
	Task<Hotel?> GetHotelByIdAsync(int hotelId);

	// 4️⃣ Update a hotel
	Task UpdateHotelAsync(Hotel hotel);

	// 5️⃣ Delete a hotel
	Task DeleteHotelAsync(Hotel hotel);
}
