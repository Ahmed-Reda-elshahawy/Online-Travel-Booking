using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Common.Helpers.Flights;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Common.Enums.Flight;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.CreateFlightBooking;

public class CreateFlightBookingCommandHandler : IRequestHandler<CreateFlightBookingCommand, Result<CreateFlightBookingResponse>>
{
    private readonly IFlightBookingsRepository flightBookingsRepository;
    private readonly IFlightRepository flightRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IBookingRepository bookingRepository;
    private readonly IPassengerRepository passengerRepository;

    public CreateFlightBookingCommandHandler(
        IFlightBookingsRepository flightBookingsRepository, 
        IFlightRepository flightRepository, 
        ICurrentUserService currentUserService, 
        IBookingRepository bookingRepository,
        IPassengerRepository passengerRepository)
    {
        this.flightBookingsRepository = flightBookingsRepository;
        this.flightRepository = flightRepository;
        this.currentUserService = currentUserService;
        this.bookingRepository = bookingRepository;
        this.passengerRepository = passengerRepository;
    }

    public async Task<Result<CreateFlightBookingResponse>> Handle(CreateFlightBookingCommand command, CancellationToken cancellationToken)
    {
        // Validate Flight
        var flight = await flightRepository.GetByIdAsync(command.Request.FlightId);
        if (flight == null)
            return Result.Failure<CreateFlightBookingResponse>(new Error("FlightNotFound", "Flight not found."));

        if (flight.Status != FlightStatus.Scheduled)
            return Result.Failure<CreateFlightBookingResponse>(new Error("FlightNotAvailable", "Flight is not available."));
        
        if (flight.DepartureTimeUtc < DateTime.UtcNow)
            return Result.Failure<CreateFlightBookingResponse>(new Error("FlightDeparted", "Flight has already departed."));

        var availableSeats = await flightRepository.GetAvailableSeatsAsync(command.Request.FlightId, flight.TotalSeats);
        if (availableSeats.Count < command.Request.Passengers.Count)
            return Result.Failure<CreateFlightBookingResponse>(new Error("InsufficientSeats", "Not enough seats available."));

        // Validate Seats
        var seatValidationResult = await ValidateSeats(flight.Id, command.Request.Passengers);
        if (!seatValidationResult.IsSuccess)
            return Result.Failure<CreateFlightBookingResponse>(seatValidationResult.Error);

        // Create BaseBooking
        var booking = new BaseBooking
        {
            UserId = currentUserService.UserId,
            BookingReference = FlightsHelpers.GenerateReference(),
            Category = BookingCategory.Flight,
            Status = BookingStatus.Pending,
            BookingDate = DateTime.UtcNow,
            PaymentStatus = PaymentStatus.Pending,
            PaymentMethod = command.Request.PaymentMethod,
            Currency = flight.Currency,
            TotalPrice = 0
        };
        await bookingRepository.AddBookingAsync(booking);

        // Create Passengers and FlightBookings
        decimal totalPrice = 0;
        foreach (var passengerDto in command.Request.Passengers)
        {
            // Create Passenger
            var passenger = new Passenger
            {
                BookingId = booking.Id,
                FirstName = passengerDto.FirstName,
                LastName = passengerDto.LastName,
                DateOfBirth = passengerDto.DateOfBirth,
                Email = passengerDto.Email,
                Phone = passengerDto.Phone,
                PassportNumber = passengerDto.PassportNumber,
                MealPreference = passengerDto.MealPreference,
                CreatedAt = DateTime.UtcNow,
            };
            await passengerRepository.AddPassengerAsync(passenger);

            // Calculate Price
            var ticketPrice = FlightsHelpers.GetPrice(flight, passengerDto.CabinClass);
            var seatCharge = 0m;
            if (!string.IsNullOrEmpty(passengerDto.SeatNumber))
            {
                seatCharge = SeatHelper.GetExtraCharge(passengerDto.SeatNumber!, flight);
            }
            var taxes = (ticketPrice + seatCharge) * 0.20m;
            var total = ticketPrice + seatCharge + taxes;
            totalPrice += total;

            // Create FlightBooking
            var flightBooking = new FlightBooking
            {
                BookingId = booking.Id,
                FlightId = flight.Id,
                PassengerId = passenger.Id,
                SeatNumber = passengerDto.SeatNumber!,
                CabinClass = passengerDto.CabinClass,
                TicketPrice = ticketPrice,
                TaxesAndFees = taxes,
                TotalPrice = total,
                Status = BookingStatus.Pending
            };
            await flightBookingsRepository.CreateFlightBookingAsync(flightBooking);
        }

        // Update Booking Total Price
        booking.TotalPrice = totalPrice;
        await bookingRepository.UpdateBookingAsync(booking);

        // Prepare Response
        var response = new CreateFlightBookingResponse
        {
            BookingId = booking.Id,
            BookingReference = booking.BookingReference,
            Status = booking.Status.ToString(),
            TotalPrice = booking.TotalPrice,
            Currency = booking.Currency,
            Message = "Booking created successfully."
        };

        return Result.Success<CreateFlightBookingResponse>(response);
    }


    private async Task<Result> ValidateSeats(int flightId, List<PassengerDto> passengers)
    {
        var passengersWithSeats = passengers.Where(p => !string.IsNullOrEmpty(p.SeatNumber)).ToList();

        if (!passengersWithSeats.Any())
            return Result.Success(); // No seats to validate

        // Check for duplicate seats in request
        var seatNumbers = passengersWithSeats.Select(p => p.SeatNumber).ToList();
        if (seatNumbers.Count != seatNumbers.Distinct().Count())
            return Result.Failure(new Error("DuplicateSeats", "Cannot select the same seat for multiple passengers."));

        // Check if seats are already booked
        foreach (var passenger in passengersWithSeats)
        {
            var isBooked = await flightRepository.IsSeatBookedAsync(flightId, passenger.SeatNumber!);

            if (isBooked)
                return Result.Failure(new Error("SeatUnavailable", $"Seat {passenger.SeatNumber} is already booked."));
        }

        return Result.Success();
    }
}
