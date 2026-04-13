using OnlineTravelBooking.Domain.Entities.Cars;
using OnlineTravelBooking.MVC.Models;

namespace OnlineTravelBooking.MVC.Services
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
        Task<int> CreateCarAsync(Car car);
        Task<int> UpdateCarAsync(Car car);
        Task<int> DeleteCarAsync(int id);
        Task<IEnumerable<Car>> SearchCarsByBrandAsync(string brand);
        Task<IEnumerable<Car>> SearchCarsAsync(
            string? brand = null,
            string? model = null,
            string? type = null,
            string? transmissionType = null,
            string? fuelType = null,
            int? minSeats = null,
            int? maxSeats = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isAvailable = null);
    }

}

