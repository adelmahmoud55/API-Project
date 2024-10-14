using LinkDev.Talabat.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specification
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey>
         where TEntity : BaseAuditableEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new() /*[]*/;  // if u have property that it will be intialized in the constructors with the same value every time u can intialize it here

        public Expression<Func<TEntity, object>>? OrderBy { get; set; } = null; // both prodcts are nullable cuz we will sort against one of them
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; } = null;

       

        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }
       

        protected BaseSpecification(Expression<Func<TEntity, bool>>? CriteriaExpression) //used with get all enrities
        {
            Criteria = CriteriaExpression;
            //Includes = new List<Expression<Func<TEntity, object>>>();// intialized in the property New()
        }

        protected BaseSpecification(TKey id) //used with Speifc entity
        {
            Criteria = E => E.Id.Equals(id);
            //Includes = new List<Expression<Func<TEntity, object>>>(); // intialized in the property new()

        }


        private protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;

        }

        private protected virtual void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }


        private protected virtual void AddIncludes() //to be reached by the derived classes , and to inherit it as private
        {

        }

        private protected virtual void AddPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }

      
    }
}
