using FluentValidation;
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
    public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            RuleFor(x => x.CarDto.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters");

            RuleFor(x => x.CarDto.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(50).WithMessage("Model cannot exceed 50 characters");

            RuleFor(x => x.CarDto.Type)
                .NotEmpty().WithMessage("Type is required")
                .Must(BeValidCarType).WithMessage("Invalid car type");

            RuleFor(x => x.CarDto.TransmissionType)
                .NotEmpty().WithMessage("Transmission type is required")
                .Must(BeValidTransmission).WithMessage("Invalid transmission type");

            RuleFor(x => x.CarDto.FuelType)
                .NotEmpty().WithMessage("Fuel type is required")
                .Must(BeValidFuelType).WithMessage("Invalid fuel type");

            RuleFor(x => x.CarDto.SeatingCapacity)
                .GreaterThan(0).WithMessage("Seating capacity must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Seating capacity cannot exceed 20");

            RuleFor(x => x.CarDto.PricePerDay)
                .GreaterThan(0).WithMessage("Price per day must be greater than 0");

            RuleFor(x => x.CarDto.Year)
                .GreaterThan(1900).WithMessage("Year must be greater than 1900")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage("Year cannot be in the future");

            RuleFor(x => x.CarDto.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required");
        }

        private bool BeValidCarType(string type)
        {
            var validTypes = new[] { "Sedan", "SUV", "Hatchback", "Coupe", "Convertible", "Van", "Truck" };
            return validTypes.Contains(type, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidTransmission(string transmission)
        {
            var validTypes = new[] { "Automatic", "Manual" };
            return validTypes.Contains(transmission, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidFuelType(string fuelType)
        {
            var validTypes = new[] { "Petrol", "Diesel", "Electric", "Hybrid" };
            return validTypes.Contains(fuelType, StringComparer.OrdinalIgnoreCase);
        }
    }
}
