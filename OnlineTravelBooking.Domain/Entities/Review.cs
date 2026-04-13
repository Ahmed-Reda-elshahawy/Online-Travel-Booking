
using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Domain.Entities;

public class Review : BaseEntity
{
	public int UserId { get; set; }
	public int BookingId { get; set; }
	public int Rating { get; set; }
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
	public int ItemId {  get; set; } //category
    public Category? ReviewCategory {get; set;}
	public ApplicationUser? User { get; set; }
	public BaseBooking? booking { get; set; }
}
