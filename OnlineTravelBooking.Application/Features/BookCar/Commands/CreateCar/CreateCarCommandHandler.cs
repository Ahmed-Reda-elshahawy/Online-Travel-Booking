using AutoMapper;
using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.DTOs.CarDtos;
using OnlineTravelBooking.Application.ViewModels;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Cars;


namespace OnlineTravelBooking.Application.Features.BookCar.Commands.CreateCar
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, ResultViewModel<CarDto>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public CreateCarCommandHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<CarDto>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CarDto;

            var car = new Car
            {
                Brand = dto.Brand,
                Model = dto.Model,
                Type = dto.Type,
                TransmissionType = dto.TransmissionType,
                FuelType = dto.FuelType,
                SeatingCapacity = dto.SeatingCapacity,
                PricePerDay = dto.PricePerDay,
                Currency = dto.Currency,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                Year = dto.Year,
                LicensePlate = dto.LicensePlate,
                Color = dto.Color,
                CreatedAt = DateTime.UtcNow
            };
            car.CreatedAt = DateTime.UtcNow;
            

            var createdCar = await _carRepository.AddAsync(car);
            await _carRepository.SaveChangesAsync();
           
            var booking = new BaseBooking
            {
                CarId = createdCar.Id,              
                UserId = 1,            
                Category = BookingCategory.Car,
                Status = BookingStatus.Confirmed,
                BookingDate = DateTime.UtcNow,
                TotalPrice = createdCar.PricePerDay,
                Currency = createdCar.Currency,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = "Strip"
            };

            await _bookingRepository.AddBookingAsync(booking);

            var carDto = _mapper.Map<CarDto>(createdCar);

            return ResultViewModel<CarDto>.Ok(carDto);
        }
    }
}
