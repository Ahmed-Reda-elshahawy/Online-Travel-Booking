using FluentValidation;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetAvailableSeats;

public class GetAvailableSeatsQueryValidator : AbstractValidator<GetAvailableSeatsQuery>
{
    public GetAvailableSeatsQueryValidator()
    {
        RuleFor(query => query.FlightId)
            .NotEmpty().GreaterThan(0).WithMessage("Flight Id Not Valid.");
    }
}
