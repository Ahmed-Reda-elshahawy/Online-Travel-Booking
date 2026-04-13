using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookCar.Commands.UpdateCar
{
    public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
    {
        public UpdateCarCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid car ID");

            RuleFor(x => x.CarDto.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters");

            RuleFor(x => x.CarDto.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(50).WithMessage("Model cannot exceed 50 characters");

            RuleFor(x => x.CarDto.SeatingCapacity)
                .GreaterThan(0).WithMessage("Seating capacity must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Seating capacity cannot exceed 20");

            RuleFor(x => x.CarDto.PricePerDay)
                .GreaterThan(0).WithMessage("Price per day must be greater than 0");
        }
    }

}
