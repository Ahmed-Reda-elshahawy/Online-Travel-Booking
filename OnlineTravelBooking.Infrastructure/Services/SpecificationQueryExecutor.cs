using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Infrastructure.Persistence;

namespace OnlineTravelBooking.Infrastructure.Services;

public class SpecificationQueryExecutor
	: ISpecificationQueryExecutor
{
	private readonly ApplicationDbContext _context;

	public SpecificationQueryExecutor(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<T>> ListAsync<T>(
		ISpecification<T> specification,
		CancellationToken cancellationToken)
		where T : BaseEntity
	{
		var query = SpecificationEvaluator<T>
			.GetQuery(_context.Set<T>().AsQueryable(), specification);

		return await query.ToListAsync(cancellationToken);
	}
}

