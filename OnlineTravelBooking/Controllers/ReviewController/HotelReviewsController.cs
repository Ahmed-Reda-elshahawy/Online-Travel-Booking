using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;
using OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Command;
using OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Query;
using OnlineTravelBooking.Dtos;

namespace OnlineTravelBooking.Controllers.ReviewController
{
	[Route("api/[controller]")]
	[ApiController]
	public class HotelReviewsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public HotelReviewsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// =====================================
		// Create Hotel Review
		// =====================================
		[HttpPost]
		public async Task<IActionResult> CreateHotelReview(
			int hotelId,
			[FromBody] CreateHotelReviewRequest request,
			CancellationToken cancellationToken)
		{
			var command = new CreateHotelReviewCommand(
				UserId: request.UserId,
				BookingId: request.BookingId,
				HotelId: hotelId,
				Rating: request.Rating,
				Title: request.Title,
				Body: request.Body
			);

			Result<int> result = await _mediator.Send(command, cancellationToken);

			if (result.IsFailure)
				return HandleFailure(result);

			return Ok(new
			{
				ReviewId = result.Value,
				Message = "Hotel review created successfully"
			});
		}
		//===============================
		//GetAll Reviews by HotelId
		//================================
		[HttpGet]
		public async Task<IActionResult> GetHotelReviews(
		int hotelId,CancellationToken cancellationToken)
		{
			var query = new GetHotelReviewsQuery(hotelId);

			Result<List<HotelReviewDto>> result =
				await _mediator.Send(query, cancellationToken);

			if (result.IsFailure)
				return HandleFailure(result);

			return Ok(result.Value);
		}

		// =====================================
		// Common error handling
		// =====================================
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
