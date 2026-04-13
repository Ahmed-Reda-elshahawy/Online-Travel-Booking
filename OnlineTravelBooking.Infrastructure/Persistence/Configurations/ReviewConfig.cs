
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

public class ReviewConfig : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.HasOne(x => x.User)
			.WithMany()
			.HasForeignKey(x => x.UserId);
		builder.HasOne(x => x.ReviewCategory)
			.WithMany()
			.HasForeignKey(x => x.ItemId);
		builder.HasOne(x => x.booking)
			.WithOne(b => b.review)
			.HasForeignKey<Review>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
