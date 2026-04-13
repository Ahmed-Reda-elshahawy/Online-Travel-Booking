
using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;

namespace OnlineTravelBooking.Application.Features.ReviewsCategories.HotelReviews.Query
{
	public record GetHotelReviewsQuery(int HotelId)
	: IRequest<Result<List<HotelReviewDto>>>;

}
