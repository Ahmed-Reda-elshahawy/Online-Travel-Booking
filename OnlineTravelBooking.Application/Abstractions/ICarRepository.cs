using OnlineTravelBooking.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Abstractions
{
    public interface ICarRepository
    {
        Task<Car?> GetByIdAsync(int id);
        Task<(List<Car> Cars, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<Car> AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Car car);
        Task<int> SaveChangesAsync();
        Task<(List<Car> Cars, int TotalCount)> SearchCarsAsync(
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
            int pageSize);
        Task<(List<Car> Cars, int TotalCount)> GetCarsByBrandAsync(string brand, int pageNumber, int pageSize);
        Task<List<string>> GetAllBrandsAsync();
    }

}
