using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Domain.Common.Enums.Flight;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.DeleteFlight;

public class DeleteFlightCommandHandler : IRequestHandler<DeleteFlightCommand, Result>
{
    private readonly IFlightRepository flightRepository;

    public DeleteFlightCommandHandler(IFlightRepository flightRepository)
    {
        this.flightRepository = flightRepository;
    }

    public async Task<Result> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
    {
        // Get the flight by Id and check if it exists
        var flight = await flightRepository.GetByIdAsync(request.Id);
        if (flight == null)
            return Result.Failure(new Error("FlightNotFound", "Flight not found."));

        // Check for existing bookings before deletion
        var hasBookings = await flightRepository.HasBookingsAsync(request.Id);
        if (hasBookings)
            return Result.Failure(new Error("FlightHasBookings", "Cannot delete flight with existing bookings."));

        // Check if Completed or Departed
        if (flight.Status == FlightStatus.Completed || flight.Status == FlightStatus.Departed)
            return Result.Failure(new Error("InvalidFlightStatus", "Cannot delete a flight that is completed or departed."));

        // Check if the flight date is in the past
        if (flight.DepartureTimeUtc < DateTime.UtcNow)
            return Result.Failure(new Error("FlightInPast", "Cannot delete a flight that has already departed."));

        // Delete the flight using Soft Delete (Can delete scheduled or cancelled flights with no bookings)
        await flightRepository.SoftDeleteAsync(request.Id);

        return Result.Success("Flight deleted successfully.");
    }
}
