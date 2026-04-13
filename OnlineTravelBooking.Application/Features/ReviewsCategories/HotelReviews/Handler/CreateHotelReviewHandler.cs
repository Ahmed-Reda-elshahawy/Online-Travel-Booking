
using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Command;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;

namespace OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Handler;

public class CreateHotelReviewHandler
	: IRequestHandler<CreateHotelReviewCommand, Result<int>>
{
	private readonly IReviewService _reviewService;

	public CreateHotelReviewHandler(IReviewService reviewService)
	{
		_reviewService = reviewService;
	}

	public async Task<Result<int>> Handle(
		CreateHotelReviewCommand request,
		CancellationToken cancellationToken)
	{
		return await _reviewService.CreateReviewAsync(
			userId: request.UserId,
			bookingId: request.BookingId,
			itemId: request.HotelId,
			rating: request.Rating,
			title: request.Title,
			body: request.Body
		);
	}
}
