using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> dbset , ISpecifications<TEntity> spec)
        {
            var query = dbset;
            
            //Filtering
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            //Sorting
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);


            //Skip And Take(Pagination)
            if(spec.IsPaginationEnabled == true)
                query = query.Skip(spec.Skip).Take(spec.Take);
            
            //Adding Includes to query
            query = spec.Includes.Aggregate(query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            
            return query;
        }
    }
}
