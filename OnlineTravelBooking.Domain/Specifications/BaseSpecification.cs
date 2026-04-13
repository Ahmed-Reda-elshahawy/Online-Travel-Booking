using System.Linq.Expressions;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.Base;

namespace OnlineTravelBooking.Domain.Specifications;

public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
	
{
	public Expression<Func<T, bool>> Criterea { get; set; }

	public List<Expression<Func<T, object>>> Includes { get; set; }
		= new();

	public Expression<Func<T, object>> orderBy { get; set; }

	public Expression<Func<T, object>> orderbydesc { get; set; }

	public int Take { get; set; }

	public int Skip { get; set; }

	public bool IsPaginationEnabled { get; set; }

	public BaseSpecification()
	{
	}

	public BaseSpecification(Expression<Func<T, bool>> criteria)
	{
		Criterea = criteria;
	}

	protected void AddInclude(Expression<Func<T, object>> includeExpression)
	{
		Includes.Add(includeExpression);
	}

	protected void ApplyPaging(int skip, int take)
	{
		Skip = skip;
		Take = take;
		IsPaginationEnabled = true;
	}

	protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
	{
		orderBy = orderByExpression;
	}

	protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
	{
		orderbydesc = orderByDescExpression;
	}
}

