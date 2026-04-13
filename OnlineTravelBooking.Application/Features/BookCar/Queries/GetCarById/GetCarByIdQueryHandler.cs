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

namespace OnlineTravelBooking.Application.Features.BookCar.Queries.GetCarById
{
    public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, ResultViewModel<CarDto>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public GetCarByIdQueryHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<CarDto>> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetByIdAsync(request.Id);

            if (car == null)
            {
                return ResultViewModel<CarDto>.Fail($"Car with ID {request.Id} not found");
            }

            var carDto = _mapper.Map<CarDto>(car);

            return ResultViewModel<CarDto>.Ok(carDto, "Car retrieved successfully", 200);
        }
    }
}
