using MediatR;
using OnlineTravelBooking.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookCar.Commands.DeleteCar
{
    public record DeleteCarCommand(int Id) : IRequest<ResultViewModel<bool>>;

}
