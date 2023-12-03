using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Data;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Repository;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable repositories;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
            repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Key = typeof(TEntity).Name;

            if(!repositories.ContainsKey(Key))
            {
                var repository = new GenericRepository<TEntity>(_context);
                repositories.Add(Key, repository);
            }

            return repositories[Key] as IGenericRepository<TEntity>;
        }

        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
           return _context.DisposeAsync();    
        }
    }
}
