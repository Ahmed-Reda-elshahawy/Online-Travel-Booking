
using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities;

namespace OnlineTravelBooking.Infrastructure.Services;

public class ReviewService : IReviewService
{
	
		private readonly IApplicationDbContext _context;

	public ReviewService(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<int>> CreateReviewAsync(
		int userId,
		int bookingId,
		int itemId,
		//Category category,
		int rating,
		string title,
		string body)
	{
		// ===============================
		// 1️- Rating Validation
		// ===============================
		if (rating < 1 || rating > 5)
			return Result.Failure<int>(
				Error.Validation("Review.Rating", "Rating must be between 1 and 5"));

		// ===============================
		// 2️- Booking Validation
		// ===============================
		var booking = await _context.bookings
			.FirstOrDefaultAsync(b => b.Id == bookingId);

		if (booking == null)
			return Result.Failure<int>(
				Error.NotFound("Booking.NotFound", "Booking not found"));

		if (booking.UserId != userId)
			return Result.Failure<int>(
				Error.Conflict("Review.UserMismatch", "User does not own this booking"));

		if (booking.Status != BookingStatus.Confirmed)
			return Result.Failure<int>(
				Error.Conflict("Review.BookingNotCompleted", "Booking must be completed"));

		// ===============================
		// 3️- Category ↔ Booking Validation
		// ===============================
		bool validCategory = false;

		if (booking.HotelId == itemId)
		{
			validCategory = true;
		}
		else if (booking.Id == itemId)
		{
			validCategory = true;
		}
		else if (booking.CarId == itemId)
		{
			validCategory = true;
		}
		else if (booking.TourId == itemId)
		{
			validCategory = true;
		}

		if (!validCategory)
		{
			return Result.Failure<int>(
				Error.Validation("Review.InvalidItem", "Item does not match booking"));
		}
		// ===============================
		// 4️- Prevent Duplicate Review
		// ===============================
		bool alreadyReviewed = await _context.reviews.AnyAsync(r =>
			r.BookingId == bookingId &&
			r.UserId == userId &&
			r.ItemId == itemId);

		if (alreadyReviewed)
			return Result.Failure<int>(
				Error.Conflict("Review.Duplicate", "Review already exists"));

		// ===============================
		// 5️- Create Review
		// ===============================
		var review = new Review
		{
			UserId = userId,
			BookingId = bookingId,
			ItemId = itemId,
			Rating = rating,
			Title = title,
			Body = body
		};

		_context.reviews.Add(review);
		await _context.SaveChangesAsync();

		return Result.Success(review.Id);
	}
}

