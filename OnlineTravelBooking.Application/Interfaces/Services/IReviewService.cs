
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities;

namespace OnlineTravelBooking.Application.Interfaces.Services;

public interface IReviewService
{
	Task<Result<int>> CreateReviewAsync(
	   int userId,
	   int bookingId,
	   int itemId,
	  // Category category,
	   int rating,
	   string title,
	   string body
   );
}
