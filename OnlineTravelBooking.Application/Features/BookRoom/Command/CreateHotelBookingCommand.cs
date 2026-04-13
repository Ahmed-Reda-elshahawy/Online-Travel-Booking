
using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;

namespace OnlineTravelBooking.Application.Features.BookRoom.Command
{
	public record CreateHotelBookingCommand(
	int UserId,
	int HotelId,
	int Nights ,
	List<BookingRoomDto> Rooms
		) : IRequest<Result<int>>;

}
