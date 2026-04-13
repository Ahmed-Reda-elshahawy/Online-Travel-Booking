
using NetTopologySuite.Geometries;

namespace OnlineTravelBooking.Application.DTOs;

public class CreateHotelDto
{
	public string Name { get; set; } = null!;  // اسم الفندق
	public string Description { get; set; } = null!;  // وصف الفندق
	public string Address { get; set; } = null!;  // العنوان
	public string City { get; set; } = null!;  // المدينة
	public string Country { get; set; } = null!;  // البلد
	public string PostalCode { get; set; } = null!;  // الرمز البريدي

	// الموقع كنقط Latitude و Longitude
	public Point Location { get; set; }

	public int StarRating { get; set; } // تقييم النجوم (من 1 إلى 5 مثلاً)

	public TimeSpan CheckInTime { get; set; } // وقت الدخول
	public TimeSpan CheckOutTime { get; set; } // وقت الخروج

	public string ContactPhone { get; set; } = null!;  // رقم الهاتف للتواصل
	public string? WebsiteURL { get; set; } // رابط الموقع الإلكتروني (اختياري)

	public ICollection<CreateRoomDto>  roomList { get; set; } = new List<CreateRoomDto>();
}
