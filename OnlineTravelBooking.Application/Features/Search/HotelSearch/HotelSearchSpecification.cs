using OnlineTravelBooking.Domain.Entities.HotelEntity;
using OnlineTravelBooking.Domain.Specifications;

namespace OnlineTravelBooking.Application.Features.Search.HotelSearch;

public class HotelSearchSpecification : BaseSpecification<Hotel>
{
	public HotelSearchSpecification(
		string? city,
		int? minRating,
		int skip,
		int take)
	{
		if (!string.IsNullOrWhiteSpace(city))
		{
			Criterea = h => h.City == city;
		}

		if (minRating.HasValue)
		{
			Criterea = Criterea == null
				? h => h.StarRating >= minRating.Value
				: h => h.StarRating >= minRating.Value && h.City == city;
		}

		AddInclude(h => h.room);
		ApplyOrderByDescending(h => h.StarRating);
		ApplyPaging(skip, take);
	}
}
