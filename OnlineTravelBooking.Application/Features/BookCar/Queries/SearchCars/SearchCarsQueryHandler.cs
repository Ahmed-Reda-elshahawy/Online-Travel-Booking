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

namespace OnlineTravelBooking.Application.Features.BookCar.Queries.SearchCars
{
    public class SearchCarsQueryHandler : IRequestHandler<SearchCarsQuery, ResultViewModel<PagedResult<CarDto>>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public SearchCarsQueryHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<PagedResult<CarDto>>> Handle(SearchCarsQuery request, CancellationToken cancellationToken)
        {
            var searchDto = request.SearchDto;

            var (cars, totalCount) = await _carRepository.SearchCarsAsync(
                searchDto.Brand,
                searchDto.Model,
                searchDto.Type,
                searchDto.TransmissionType,
                searchDto.FuelType,
                searchDto.MinSeats,
                searchDto.MaxSeats,
                searchDto.MinPrice,
                searchDto.MaxPrice,
                searchDto.IsAvailable,
                searchDto.PageNumber,
                searchDto.PageSize
            );

            var carDtos = _mapper.Map<List<CarDto>>(cars);
            var pagedResult = new PagedResult<CarDto>(carDtos, totalCount, searchDto.PageNumber, searchDto.PageSize);

            return ResultViewModel<PagedResult<CarDto>>.Ok(
                pagedResult,
                $"Found {totalCount} cars matching your criteria"
                ,200
            );
        }
    }
}
