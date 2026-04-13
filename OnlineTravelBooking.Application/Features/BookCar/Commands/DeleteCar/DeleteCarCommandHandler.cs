using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookCar.Commands.DeleteCar
{
    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, ResultViewModel<bool>>
    {
        private readonly ICarRepository _carRepository;

        public DeleteCarCommandHandler(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<ResultViewModel<bool>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetByIdAsync(request.Id);

            if (car == null)
            {
                return ResultViewModel<bool>.Fail($"Car with ID {request.Id} not found");
            }

            await _carRepository.DeleteAsync(car);
            await _carRepository.SaveChangesAsync();

            return ResultViewModel<bool>.Ok(true, "Car deleted successfully");
        }
    }
}
