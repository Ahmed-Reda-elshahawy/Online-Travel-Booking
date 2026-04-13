using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<int> Add(TEntity tentity);
        Task<int> Delete(int id);

        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<Tresult>> GetAll<Tresult>(Expression<Func<TEntity, Tresult>> selector);
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetById(int id);
        Task<int> Update(TEntity tentity);
    }
}
