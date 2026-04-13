
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;
using OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Query;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities;

namespace OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Handler;

public class GetHotelReviewsQueryHandler
	: IRequestHandler<GetHotelReviewsQuery, Result<List<HotelReviewDto>>>
{
	private readonly IApplicationDbContext _context;

	public GetHotelReviewsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<List<HotelReviewDto>>> Handle(
		GetHotelReviewsQuery request,
		CancellationToken cancellationToken)
	{
		var reviews = await _context.reviews
			.AsNoTracking()
			.Where(r =>
				r.ItemId == request.HotelId &&
				r.ReviewCategory.bookingCategory == BookingCategory.Hotel)
			.OrderByDescending(r => r.CreatedAt)
			.Select(r => new HotelReviewDto
			{
				ReviewId = r.Id,
				UserId = r.UserId,
				Rating = r.Rating,
				Title = r.Title,
				Body = r.Body,
				CreatedAt = r.CreatedAt
			})
			.ToListAsync(cancellationToken);

		return Result.Success(reviews);
	}
}
