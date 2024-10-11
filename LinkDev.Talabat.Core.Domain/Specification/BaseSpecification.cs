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
        public Expression<Func<TEntity,bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new() /*[]*/;  // if u have property that it will be intialized in the constructors with the same value every time u can intialize it here

        public BaseSpecification() //used with get all enrities
        {
            //Criteria = null;
            //Includes = new List<Expression<Func<TEntity, object>>>();// intialized in the property New()
        }

        public BaseSpecification(TKey id) //used with Speifc entity
        {
            Criteria = E => E.Id.Equals(id);
            //Includes = new List<Expression<Func<TEntity, object>>>(); // intialized in the property new()

        }
    }
}
