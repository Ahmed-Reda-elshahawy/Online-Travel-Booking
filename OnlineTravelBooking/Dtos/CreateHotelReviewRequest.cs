namespace OnlineTravelBooking.Dtos
{
	public class CreateHotelReviewRequest
	{
		public int UserId { get; set; }
		public int BookingId { get; set; }
		public int Rating { get; set; }   // 1 → 5
		public string Title { get; set; } = null!;
		public string Body { get; set; } = null!;
	}
}
