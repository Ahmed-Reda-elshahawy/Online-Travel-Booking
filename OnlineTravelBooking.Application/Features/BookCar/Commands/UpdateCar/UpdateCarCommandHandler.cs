using AutoMapper;
using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.DTOs.CarDtos;
using OnlineTravelBooking.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookCar.Commands.UpdateCar
{
    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, ResultViewModel<CarDto>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public UpdateCarCommandHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<CarDto>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetByIdAsync(request.Id);

            if (car == null)
            {
                return ResultViewModel<CarDto>.Fail($"Car with ID {request.Id} not found");
            }

            _mapper.Map(request.CarDto, car);
            car.UpdatedAt = DateTime.UtcNow;

            await _carRepository.UpdateAsync(car);
            await _carRepository.SaveChangesAsync();

            var carDto = _mapper.Map<CarDto>(car);

            return ResultViewModel<CarDto>.Ok(carDto, "Car updated successfully");
        }
    }
}
