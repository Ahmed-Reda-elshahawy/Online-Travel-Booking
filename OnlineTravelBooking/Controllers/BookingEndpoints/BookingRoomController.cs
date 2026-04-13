using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Features.BookRoom.Command;

namespace OnlineTravelBooking.Controllers.BookingEndpoints
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingRoomController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BookingRoomController(IMediator mediator)
		{
			_mediator = mediator;
		}

		
		//Create hotel booking with rooms
		
		[HttpPost]
		public async Task<IActionResult> CreateHotelBooking(
			[FromBody] CreateHotelBookingCommand command,
			CancellationToken cancellationToken)
		{
			Result<int> result = await _mediator.Send(command, cancellationToken);

			if (result.IsFailure)
				return HandleFailure(result);

			return Ok(new
			{
				BookingId = result.Value,
				Message = "Hotel booking created successfully"
			});
		}

		[HttpDelete("{bookingId}/rooms/{bookingRoomId}")]
		public async Task<IActionResult> CancelBookingRoom(
		int bookingId,
		int bookingRoomId,
		CancellationToken cancellationToken)
		{
			var command = new CancelBookingRoomCommand(
				bookingId,
				bookingRoomId);

			Result result = await _mediator.Send(command, cancellationToken);

			if (result.IsFailure)
				return HandleFailure(result);

			return Ok(new { message = "Booking room cancelled successfully" });
		}

		private IActionResult HandleFailure(Result result)
		{
			return result.Error.Code switch
			{
				var code when code.Contains("NotFound") =>
					NotFound(result.Error),

				var code when code.Contains("Validation") =>
					BadRequest(result.Error),

				var code when code.Contains("Conflict") =>
					Conflict(result.Error),

				_ => StatusCode(500, result.Error)
			};
		}


	}
}
