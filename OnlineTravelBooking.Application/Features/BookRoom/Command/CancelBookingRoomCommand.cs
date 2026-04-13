using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.BookRoom.Command
{
	public record CancelBookingRoomCommand(
	int BookingId,
	int BookingRoomId
) : IRequest<Result>;
}
