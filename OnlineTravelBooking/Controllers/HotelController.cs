using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Features.Search.HotelSearch.Query;

namespace OnlineTravelBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HotelController : ControllerBase
	{
		private readonly IMediator _mediator;

		public HotelController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchHotels(
			[FromQuery] string? city,
			[FromQuery] int? minRating,
			[FromQuery] int pageIndex = 1,
			[FromQuery] int pageSize = 10)
		{
			var query = new SearchHotelsQuery(
				city,
				minRating,
				pageIndex,
				pageSize);

			var result = await _mediator.Send(query);

			if (result.IsFailure)
				return BadRequest(result.Error);

			return Ok(result.Value);
		}
	}
}

