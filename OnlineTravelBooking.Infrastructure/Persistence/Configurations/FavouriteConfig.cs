
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

public class FavouriteConfig : IEntityTypeConfiguration<Favourite>
{
	public void Configure(EntityTypeBuilder<Favourite> builder)
	{
		builder.HasOne(x => x.FavouriteCategory)
			.WithMany()
			.HasForeignKey(x => x.ItemId);
		builder.HasOne(x => x.User)
			.WithMany()
			.HasForeignKey(x => x.UserId);
	}
}
