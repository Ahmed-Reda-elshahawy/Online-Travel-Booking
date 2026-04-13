using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Interfaces;
using OnlineTravelBooking.Domain.Entities;
using OnlineTravelBooking.Domain.Entities.Cars;
using OnlineTravelBooking.Infrastructure.Persistence;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.CarRepo
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<(List<Car> Cars, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Cars
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt);

            var totalCount = await query.CountAsync();

            var cars = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (cars, totalCount);
        }

        public async Task<Car> AddAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            return car;
        }

        public Task UpdateAsync(Car car)
        {
            _context.Cars.Update(car);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Car car)
        {
            car.IsDeleted = true;
            car.DeletedAt = DateTime.UtcNow;
            _context.Cars.Update(car);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<(List<Car> Cars, int TotalCount)> SearchCarsAsync(
            string? brand,
            string? model,
            string? type,
            string? transmissionType,
            string? fuelType,
            int? minSeats,
            int? maxSeats,
            decimal? minPrice,
            decimal? maxPrice,
            bool? isAvailable,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Cars.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(c => c.Brand.ToString().Contains(brand.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                query = query.Where(c => c.Model.ToLower().Contains(model.ToLower()));
            }

          
            

         

          

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.PricingTiers.Any(pt => pt.PricePerHour <= maxPrice.Value));
            }

          

            var totalCount = await query.CountAsync();

            var cars = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (cars, totalCount);
        }

        public async Task<(List<Car> Cars, int TotalCount)> GetCarsByBrandAsync(string brand, int pageNumber, int pageSize)
        {
            var query = _context.Cars
                .Where(c => !c.IsDeleted && c.Brand == brand)
                .OrderByDescending(c => c.CreatedAt);

            var totalCount = await query.CountAsync();

            var cars = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (cars, totalCount);
        }

        public async Task<List<string>> GetAllBrandsAsync()
        {
            return await _context.Cars
                .Where(c => !c.IsDeleted)
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(b => b)
                .ToListAsync();
        }
    }

}
