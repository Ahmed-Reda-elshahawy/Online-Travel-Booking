
using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;

namespace OnlineTravelBooking.Application.Features.Search.HotelSearch.Query;

public record SearchHotelsQuery(
	string? City,
	int? MinRating,
	int PageIndex = 1,
	int PageSize = 10
) : IRequest<Result<List<HotelDto>>>;

