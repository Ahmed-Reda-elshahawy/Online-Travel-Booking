using OnlineTravelBooking.Domain.Entities.Base;

namespace OnlineTravelBooking.Domain.Entities.HotelEntity;

public class RoomImage : BaseEntity
{
    public int RoomId { get; set; }
    public string URL { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;

    public Room? Room { get; set; }
}
