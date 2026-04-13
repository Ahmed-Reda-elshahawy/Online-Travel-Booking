using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.HotelRepos;

public class HotelRepository : IHotelRepo
{
	private readonly ApplicationDbContext _dbContext;

	public HotelRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	// 1️- Add a new hotel
	public async Task AddHotelAsync(Hotel hotel)
	{
		await _dbContext.hotels.AddAsync(hotel);
		await _dbContext.SaveChangesAsync();
	}

	// 2️- Get all hotels with pagination
	public async Task<PaginatedList<Hotel>> GetAllHotelsAsync(int pageNumber, int pageSize)
	{
		var query = _dbContext.hotels.AsNoTracking().OrderBy(h => h.Name);
		return await PaginatedList<Hotel>.Create(query, pageNumber, pageSize);
	}

	// 3️- Get hotel by ID
	public async Task<Hotel?> GetHotelByIdAsync(int hotelId)
	{
		return await _dbContext.hotels
			.AsNoTracking()
			.FirstOrDefaultAsync(h => h.Id == hotelId);
	}

	// 4️- Update a hotel
	public async Task UpdateHotelAsync(Hotel hotel)
	{
		_dbContext.hotels.Update(hotel);
		await _dbContext.SaveChangesAsync();
	}

	// 5️- Delete a hotel
	public async Task DeleteHotelAsync(Hotel hotel)
	{
		_dbContext.hotels.Remove(hotel);
		await _dbContext.SaveChangesAsync();
	}
}
