using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;
using OnlineTravelBooking.Application.Features.Search.HotelSearch.Query;
using OnlineTravelBooking.Application.Features.Search.HotelSearch;
using OnlineTravelBooking.Application.Interfaces.Services;

public class SearchHotelsQueryHandler
	: IRequestHandler<SearchHotelsQuery, Result<List<HotelDto>>>
{
	private readonly ISpecificationQueryExecutor _executor;

	public SearchHotelsQueryHandler(ISpecificationQueryExecutor executor)
	{
		_executor = executor;
	}

	public async Task<Result<List<HotelDto>>> Handle(
		SearchHotelsQuery request,
		CancellationToken cancellationToken)
	{
		int skip = (request.PageIndex - 1) * request.PageSize;

		var spec = new HotelSearchSpecification(
			request.City,
			request.MinRating,
			skip,
			request.PageSize);

		var hotels = await _executor
			.ListAsync(spec, cancellationToken);

		if (!hotels.Any())
		{
			return Result.Success(new List<HotelDto>());
		}

			var result = hotels.Select(h => new HotelDto(
			h.Id,
			h.Name,
			h.City,
			h.StarRating)).ToList();


		return Result.Success(result);
	}
}
