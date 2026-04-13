using FluentValidation;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.DeleteFlight;

public class DeleteFlightCommandValidator : AbstractValidator<DeleteFlightCommand>
{
    public DeleteFlightCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Enter A Valid Flight ID.");
    }
}
