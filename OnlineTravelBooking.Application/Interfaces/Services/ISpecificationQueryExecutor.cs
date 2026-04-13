using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.Base;

namespace OnlineTravelBooking.Application.Interfaces.Services;

public interface ISpecificationQueryExecutor
{
	Task<List<T>> ListAsync<T>(
		ISpecification<T> specification,
		CancellationToken cancellationToken)
		where T : BaseEntity;
}
