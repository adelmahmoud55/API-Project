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
    public class GenericRepository<TEntity, TKey>(StoreContext DbContext) : IGenericRepository<TEntity, TKey>
         where TEntity : BaseAuditableEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
            => withTracking? await DbContext.Set<TEntity>().ToListAsync() : 
                await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        
        //if(withTracking)
        // {
        //   return await  _dbContext.Set<TEntity>().ToListAsync();
        // }
        //
        //return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();



        public async Task<TEntity?> GetAsync(TKey id)
        {
            //if (typeof(TEntity) == typeof(Product))
            //   return await DbContext.Set<Product>().Where(p => p.Id.Equals(id)).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as TEntity;

            return await DbContext.Set<TEntity>().FindAsync(id);
        }




        public async Task AddAsync(TEntity entity) => await DbContext.Set<TEntity>().AddAsync(entity);



        public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);


        public void Delete(TEntity entity) => DbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> spec, bool withTracking = false)
        {
            return await ApplySpecification( spec).ToListAsync();   // the query that came from  SpecificationsEvaluator will be excuted here
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        #region Helpers

        private  IQueryable<TEntity> ApplySpecification( ISpecification<TEntity, TKey> spec)
        {
            return SpecificationsEvaluator<TEntity, TKey>.GetQuery(DbContext.Set<TEntity>(), spec);
        }


        #endregion
    }
}
