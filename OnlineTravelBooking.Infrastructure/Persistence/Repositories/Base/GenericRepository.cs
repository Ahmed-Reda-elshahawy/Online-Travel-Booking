using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.Base
{
    public class GenericRepository<Tentity> : IGenericRepository<Tentity> where Tentity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Tentity>> GetAll()
        {
            return await _context.Set<Tentity>().ToListAsync();
        }
        public async Task<IEnumerable<Tresult>> GetAll<Tresult>(Expression<Func<Tentity, Tresult>> selector)
        {
            return await _context.Set<Tentity>().Select(selector).ToListAsync();
        }

        public async Task<Tentity?> GetById(int id)
        {
            return await _context.Set<Tentity>().FirstOrDefaultAsync(e => e.Id == id );
        }
        public async Task<int> Add(Tentity tentity)
        {
           await  _context.Set<Tentity>().AddAsync(tentity);
           return await _context.SaveChangesAsync();

        }

        public async Task<int> Update(Tentity tentity)
        {
            _context.Set<Tentity>().Update(tentity);
            return await _context.SaveChangesAsync();

        }
        public async Task<int> Delete(int id)
        {
            _context.Set<Tentity>().Remove(await GetById(id));
            return await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Tentity>> GetAll(Expression<Func<Tentity, bool>> predicate)
        {
            return await _context.Set<Tentity>().Where(predicate).ToListAsync();
        }
    }
}
