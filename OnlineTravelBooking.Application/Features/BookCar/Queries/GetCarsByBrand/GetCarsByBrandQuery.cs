using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.CarDtos;
using OnlineTravelBooking.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookCar.Queries.GetCarsByBrand
{
    public record GetCarsByBrandQuery(string Brand, int PageNumber = 1, int PageSize = 10)
     : IRequest<ResultViewModel<PagedResult<CarDto>>>;
}
