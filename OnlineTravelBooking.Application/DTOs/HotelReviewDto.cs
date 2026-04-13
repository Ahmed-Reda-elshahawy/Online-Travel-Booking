namespace OnlineTravelBooking.Application.DTOs;

public class HotelReviewDto
{
	public int ReviewId { get; set; }
	public int UserId { get; set; }
	public int Rating { get; set; }
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
	public DateTime CreatedAt { get; set; }
}
