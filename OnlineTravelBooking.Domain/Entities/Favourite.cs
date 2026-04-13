using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Domain.Entities;

public class Favourite : BaseEntity
{
	public int UserId { get; set; }
	public int ItemId { get; set; }
	 public Category? FavouriteCategory {get; set;}
	public ApplicationUser? User { get; set; }

}
