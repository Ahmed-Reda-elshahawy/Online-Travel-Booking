using OnlineTravelBooking.Domain.Entities.Base;
using NetTopologySuite.Geometries;

namespace OnlineTravelBooking.Domain.Entities.HotelEntity;

public class Hotel : BaseEntity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string Addresse { get; set; }
	public string City { get; set; }
	public string Country { get; set; }
	public string PostalCode { get; set; }
	public Point? Location { get; set; }
	public int StarRating { get; set; }
	public TimeSpan CheckInTime { get; set; }
   public TimeSpan CheckOutTime { get; set; }
   public string ContactPhone { get; set; }
   public string WebsiteURL { get; set; }

	public ICollection<Room> room { get; set; } = new HashSet<Room>();

}
