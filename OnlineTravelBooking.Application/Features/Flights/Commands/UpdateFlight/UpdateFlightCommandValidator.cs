using FluentValidation;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.UpdateFlight;

public class UpdateFlightCommandValidator : AbstractValidator<UpdateFlightCommand>
{
    public UpdateFlightCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Flight ID must be greater than 0.");

        RuleFor(x => x.FlightNumber)
            .NotEmpty().WithMessage("Flight number is required.")
            .MaximumLength(20).WithMessage("Flight number must not exceed 20 characters.")
            .Matches("^[A-Z]{2}[0-9]{1,4}$").WithMessage("Flight number must be in format: AA123");

        RuleFor(x => x.AirlineName)
            .NotEmpty().WithMessage("Airline name is required.")
            .MaximumLength(100).WithMessage("Airline name must not exceed 100 characters.");

        RuleFor(x => x.AircraftType)
            .NotEmpty().WithMessage("Aircraft type is required.")
            .MaximumLength(100).WithMessage("Aircraft type must not exceed 100 characters.");

        // Origin validation
        RuleFor(x => x.OriginAirportCode)
            .NotEmpty().WithMessage("Origin airport code is required.")
            .Length(3).WithMessage("Airport code must be 3 characters.")
            .Matches("^[A-Z]{3}$").WithMessage("Airport code must be 3 uppercase letters.");

        RuleFor(x => x.OriginCity)
            .NotEmpty().WithMessage("Origin city is required.")
            .MaximumLength(100).WithMessage("City name must not exceed 100 characters.");

        RuleFor(x => x.OriginLatitude)
            .InclusiveBetween(-90, 90)
            .When(x => x.OriginLatitude.HasValue)
            .WithMessage("Origin latitude must be between -90 and 90.");

        RuleFor(x => x.OriginLongitude)
            .InclusiveBetween(-180, 180)
            .When(x => x.OriginLongitude.HasValue)
            .WithMessage("Origin longitude must be between -180 and 180.");

        // Destination validation
        RuleFor(x => x.DestinationAirportCode)
            .NotEmpty().WithMessage("Destination airport code is required.")
            .Length(3).WithMessage("Airport code must be 3 characters.")
            .Matches("^[A-Z]{3}$").WithMessage("Airport code must be 3 uppercase letters.");

        RuleFor(x => x.DestinationCity)
            .NotEmpty().WithMessage("Destination city is required.")
            .MaximumLength(100).WithMessage("City name must not exceed 100 characters.");

        RuleFor(x => x.DestinationLatitude)
            .InclusiveBetween(-90, 90)
            .When(x => x.DestinationLatitude.HasValue)
            .WithMessage("Destination latitude must be between -90 and 90.");

        RuleFor(x => x.DestinationLongitude)
            .InclusiveBetween(-180, 180)
            .When(x => x.DestinationLongitude.HasValue)
            .WithMessage("Destination longitude must be between -180 and 180.");

        // Schedule validation
        RuleFor(x => x.DepartureTimeUtc)
            .NotEmpty().WithMessage("Departure time is required.");

        RuleFor(x => x.ArrivalTimeUtc)
            .NotEmpty().WithMessage("Arrival time is required.")
            .GreaterThan(x => x.DepartureTimeUtc).WithMessage("Arrival time must be after departure time.");

        // Seat configuration validation
        RuleFor(x => x.FirstClassRows)
            .GreaterThanOrEqualTo(0).WithMessage("First class rows must be 0 or greater.");

        RuleFor(x => x.BusinessClassRows)
            .GreaterThanOrEqualTo(0).WithMessage("Business class rows must be 0 or greater.");

        RuleFor(x => x.EconomyClassRows)
            .GreaterThan(0).WithMessage("Economy class rows must be greater than 0.");

        // Currency validation
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be 3 characters (ISO code).")
            .Matches("^[A-Z]{3}$").WithMessage("Currency must be 3 uppercase letters.");

        // Pricing validation
        RuleFor(x => x.EconomyPrice)
            .GreaterThan(0).WithMessage("Economy price must be greater than 0.");

        RuleFor(x => x.BusinessPrice)
            .GreaterThan(0).WithMessage("Business price must be greater than 0.")
            .GreaterThan(x => x.EconomyPrice).WithMessage("Business price must be higher than Economy price.");

        RuleFor(x => x.FirstClassPrice)
            .GreaterThan(0).WithMessage("First class price must be greater than 0.")
            .GreaterThan(x => x.BusinessPrice).WithMessage("First class price must be higher than Business price.");

        // Status validation
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(status => new[] { "Scheduled", "Delayed", "Cancelled", "Boarding", "Departed", "Completed" }
                .Contains(status))
            .WithMessage("Invalid flight status.");
    }
}
