
using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.HotelRepos;

public class RoomRepo : IRoomRepo
{
	private readonly ApplicationDbContext _dbContext;

	public RoomRepo(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public async Task AddRoomsAsync(List<Room> rooms)
	{
		await _dbContext.rooms.AddRangeAsync(rooms);
		await _dbContext.SaveChangesAsync();
	}


	public async Task<List<Room>> GetAllRoomsByHotelIdAsync(int hotelId)
	{
		return await _dbContext.rooms
		   .AsNoTracking()
		   .Where(r => r.HotelId == hotelId)
		   .OrderBy(r => r.RoomNum)
		   .ToListAsync();
	}

	public async Task<Room?> GetRoomByIdAsync(int roomId)
	{
		return await _dbContext.rooms
			.FirstOrDefaultAsync(r => r.Id == roomId);
	}

	// ===============================
	// Update Room
	// ===============================
	public async Task UpdateRoomAsync(Room room)
	{
		_dbContext.rooms.Update(room);
		await _dbContext.SaveChangesAsync();
	}

	// ===============================
	// Delete Room
	// ===============================
	public async Task DeleteRoomAsync(Room room)
	{
		_dbContext.rooms.Remove(room);
		await _dbContext.SaveChangesAsync();
	}
}
