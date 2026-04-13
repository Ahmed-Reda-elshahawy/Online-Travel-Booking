using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Domain.Entities.Cars;
using OnlineTravelBooking.Infrastructure.Persistence;
using OnlineTravelBooking.MVC.Models;
using OnlineTravelBooking.MVC.Services;

public class CarService : ICarService
{
    private readonly ApplicationDbContext _context;

    public CarService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        return await _context.Cars
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Car?> GetCarByIdAsync(int id)
    {
        return await _context.Cars
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateCarAsync(Car car)
    {
        car.CreatedAt = DateTime.UtcNow;

        car.IsDeleted = false;

        await _context.Cars.AddAsync(car);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateCarAsync(Car car)
    {
        car.UpdatedAt = DateTime.UtcNow;
        _context.Cars.Update(car);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteCarAsync(int id)
    {
        var car = await GetCarByIdAsync(id);
        if (car == null)
        {
            return 0;
        }

        car.IsDeleted = true;
        car.DeletedAt = DateTime.UtcNow;

        _context.Cars.Update(car);
        return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Car>> SearchCarsByBrandAsync(string brand)
    {
        return await _context.Cars
            .Where(c => !c.IsDeleted && c.Brand.ToLower().Contains(brand.ToLower()))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Car>> SearchCarsAsync(
        string? brand = null,
        string? model = null,
        string? type = null,
        string? transmissionType = null,
        string? fuelType = null,
        int? minSeats = null,
        int? maxSeats = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isAvailable = null)
    {
        var query = _context.Cars.Where(c => !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(c => c.Brand.ToLower().Contains(brand.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(model))
        {
            query = query.Where(c => c.Model.ToLower().Contains(model.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(c => c.Type.ToLower() == type.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(transmissionType))
        {
            query = query.Where(c => c.TransmissionType.ToLower() == transmissionType.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(fuelType))
        {
            query = query.Where(c => c.FuelType.ToLower() == fuelType.ToLower());
        }

        if (minSeats.HasValue)
        {
            query = query.Where(c => c.SeatingCapacity >= minSeats.Value);
        }

        if (maxSeats.HasValue)
        {
            query = query.Where(c => c.SeatingCapacity <= maxSeats.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(c => c.PricePerDay >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(c => c.PricePerDay <= maxPrice.Value);
        }



        return await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
