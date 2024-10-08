using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Repositories.Generic_Repository
{
    internal static class SpecificationsEvaluator<TEntity, TKey> 
         where TEntity : BaseAuditableEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specification)
        {
            var query = inputQuery; // _dbContext.Set<Product>()

            if (specification.Criteria != null) // P => P.Id == 1 or will be null if we don't have any criteria {return all of the entity}
            {
                query = query.Where(specification.Criteria);
            }

            // query =  _dbContext.Set<Product>().Where( P => P.Id == 1); 
            // Includes expressions
            // 1. P => P.Brand
            // 2. P => P.Category

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            // query =  _dbContext.Set<Product>().Where( P => P.Id == 1).Include(P => P.Brand);
            // query =  _dbContext.Set<Product>().Where( P => P.Id == 1).Include(P => P.Brand).Include(P => P.Category);
            // w 3ady momkn ykon mn 8er where condition

            // this differed execution , it will be executed when we call the query.ToList() or query.FirstOrDefault() or query.Any()


            return query;
        }

    }
}
