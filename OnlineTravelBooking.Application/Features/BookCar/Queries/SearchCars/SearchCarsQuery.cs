using MediatR;
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
    public record SearchCarsQuery(CarSearchDto SearchDto) : IRequest<ResultViewModel<PagedResult<CarDto>>>;

}
