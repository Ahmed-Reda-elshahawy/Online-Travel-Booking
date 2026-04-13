using FluentValidation;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.CreateFlightBooking;

public class CreateFlightBookingCommandValidator : AbstractValidator<CreateFlightBookingCommand>
{
    public CreateFlightBookingCommandValidator()
    {
        RuleFor(x => x.Request.FlightId)
            .GreaterThan(0).WithMessage("Flight ID is required.");

        RuleFor(x => x.Request.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.");

        RuleFor(x => x.Request.Passengers)
            .NotEmpty().WithMessage("At least one passenger is required.")
            .Must(p => p.Count <= 9).WithMessage("Maximum 9 passengers per booking.");

        RuleForEach(x => x.Request.Passengers).ChildRules(passenger =>
        {
            passenger.RuleFor(p => p.FirstName).NotEmpty().MaximumLength(100);
            passenger.RuleFor(p => p.LastName).NotEmpty().MaximumLength(100);
            passenger.RuleFor(p => p.Email).NotEmpty().EmailAddress();
            passenger.RuleFor(p => p.Phone).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
            passenger.RuleFor(p => p.CabinClass).IsInEnum();
        });
    }
}
