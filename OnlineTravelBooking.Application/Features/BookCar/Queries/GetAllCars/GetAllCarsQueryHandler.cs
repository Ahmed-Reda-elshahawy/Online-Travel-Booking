using AutoMapper;
using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.CarDtos;
using OnlineTravelBooking.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookCar.Queries.GetAllCars
{
    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, ResultViewModel<PagedResult<CarDto>>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public GetAllCarsQueryHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<PagedResult<CarDto>>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            var (cars, totalCount) = await _carRepository.GetAllPagedAsync(request.PageNumber, request.PageSize);

            var carDtos = _mapper.Map<List<CarDto>>(cars);
            var pagedResult = new PagedResult<CarDto>(carDtos, totalCount, request.PageNumber, request.PageSize);

            return ResultViewModel<PagedResult<CarDto>>.Ok(
                pagedResult,
                "Cars retrieved successfully"
            );
        }
    }
}
