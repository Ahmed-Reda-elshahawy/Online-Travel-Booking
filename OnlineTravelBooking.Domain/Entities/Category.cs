
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities.Base;

namespace OnlineTravelBooking.Domain.Entities;

public class Category : BaseEntity
{
	public BookingCategory bookingCategory { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string ImageURL { get; set; }
	public int DisplayURL { get; set; }
	public bool IsActive { get; set; }
	
}
