
using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Command;

public record CreateHotelReviewCommand(
	int UserId,
	int BookingId,
	int HotelId,
	int Rating,
	string Title,
	string Body
) : IRequest<Result<int>>;
