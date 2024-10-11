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
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> spec)
        {
            var query = inputQuery; // _dbContext.Set<Product>()

            if (spec.Criteria != null) // P => P.Id.Eqilas(1) or will be null if we don't have any criteria {return all of the entity}
            {
                query = query.Where(spec.Criteria);
            }

            // query =  _dbContext.Set<Product>().Where( P => P.Id == 1); 
            // Includes expressions
            // 1. P => P.Brand
            // 2. P => P.Category
            //...

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            // query =  _dbContext.Set<Product>().Where( P => P.Id == 1).Include(P => P.Brand);
            // query =  _dbContext.Set<Product>().Where( P => P.Id == 1).Include(P => P.Brand).Include(P => P.Category);
            // w 3ady momkn ykon mn 8er where condition

            // this differed execution , it will be executed when we call the query.ToList() or query.FirstOrDefault() or query.Any()


            return query;
        }
        // in specification design pattern we have to create a class that will be responsible for the query execution
        // just to make the query dynamic and to make the query execution in the last step {just return the query}
        // by using any of immediate execution methods like ToList() , FirstOrDefault() , Any() , Count() , Sum() , Average() , Max() , Min() , Single() , SingleOrDefault() , First() , Last() , LastOrDefault() , ElementAt() , ElementAtOrDefault() , ToArray() , ToDictionary() , ToLookup() , ToHashSet() , ToListAsync() , FirstAsync() , FirstOrDefaultAsync() , SingleAsync() , SingleOrDefaultAsync() , LastAsync() , LastOrDefaultAsync() , ToArrayAsync() , ToDictionaryAsync() , ToLookupAsync() , ToHashSet
    }
}
