using MediatR;
using OnlineTravelBooking.Application.DTOs.CarDtos;
using OnlineTravelBooking.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookCar.Commands.CreateCar
{
    public record CreateCarCommand(CreateCarDto CarDto) : IRequest<ResultViewModel<CarDto>>;


}
