using FluentValidation;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetFlightById;

public class GetFlightByIdQueryValidator : AbstractValidator<GetFlightByIdQuery>
{
    public GetFlightByIdQueryValidator()
    {
        RuleFor(query => query.FlightId)
            .NotEmpty().GreaterThan(0).WithMessage("Flight Id Not Valid.");
    }
}
