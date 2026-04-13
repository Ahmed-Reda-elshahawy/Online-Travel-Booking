using OnlineTravelBooking.Domain.Entities.Base;
using System.Linq.Expressions;

namespace OnlineTravelBooking.Application.Abstractions
{
	public interface ISpecification<T> where T : BaseEntity
	{
		public Expression<Func<T, Boolean>> Criterea { get; set; }//set the expression inside
																  //the where in the criterea

		//sign property for include(P => P.ProductType).Include(P => P.ProductBrand)
		public List<Expression<Func<T, object>>> Includes { get; set; }//take T and return object

		//prop order by [orderby(p=>p.name)]
		public Expression<Func<T, object>> orderBy { get; set; }

		//property orderbydesc [orderby(p=>p.name)]
		public Expression<Func<T, Object>> orderbydesc { get; set; }

		//Take(2)
		public int Take { get; set; }

		//Skip(2)
		public int Skip { get; set; }
		public bool IsPaginationEnabled { get; set; }


	}
}
