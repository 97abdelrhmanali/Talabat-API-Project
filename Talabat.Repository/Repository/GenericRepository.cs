using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Data;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            #region Old return of spec
            ///if(typeof(T) == typeof(Product))
            ///    return (IEnumerable<T>) await _dbcontext.Products.Include(P => P.Brands)
            ///                                            .Include(p => p.Categories)
            ///                                            .ToListAsync();

            #endregion

            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            #region Old return of spec
            ///if (typeof(T) == typeof(Product))
            ///    return await _dbcontext.Products.Where(p => p.Id == id)
            ///                                         .Include(P => P.Brands)
            ///                                         .Include(P => P.Categories)
            ///                                         .FirstOrDefaultAsync() as T;

            #endregion
            return await _dbcontext.Set<T>().FindAsync(id);

        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
            => await _dbcontext.AddAsync(entity);

        public void Update(T entity)
            => _dbcontext.Update(entity);

        public void Delete(T entity)
            => _dbcontext.Remove(entity);
    }
}
