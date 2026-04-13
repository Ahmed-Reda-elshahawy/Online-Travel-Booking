using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.Base;

namespace OnlineTravelBooking.Infrastructure.Services;

public static class SpecificationEvaluator<T> where T : BaseEntity
{
	//func to build a query
	public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
	//parameter here is the dbset to make on
	{
		var Query = inputQuery;
		if (spec.Criterea is not null)
		{ Query = Query.Where(spec.Criterea); } // i make this line  _dbContext.Set<T>.Where(P => P.Id == id )

		if (spec.orderBy is not null)
		{
			Query = Query.OrderBy(spec.orderBy);
		}
		if (spec.orderbydesc is not null)
		{
			Query = Query.OrderByDescending(spec.orderbydesc);
		}
		if (spec.IsPaginationEnabled)
		{
			Query = Query.Skip(spec.Skip).Take(spec.Take);
		}

		Query = spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
		return Query;
		//gonna return this query structure
		//	_dbContext.Set<T>.Where(P => P.Id == id).Include(P => P.ProductType).Include(P => P.ProductBrand);
	}
}
