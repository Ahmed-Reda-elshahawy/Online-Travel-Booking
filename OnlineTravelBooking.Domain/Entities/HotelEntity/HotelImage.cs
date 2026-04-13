using OnlineTravelBooking.Domain.Entities.Base;

namespace OnlineTravelBooking.Domain.Entities.HotelEntity;

public class HotelImage : BaseEntity
{
	public int HotelId { get; set; }
    public string URL { get; set; }
	public string Caption { get; set; }

	public Hotel hotel { get; set; }
}
