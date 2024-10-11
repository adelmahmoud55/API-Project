using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts
{
    public interface ISpecification<TEntity, TKey>
         where TEntity : BaseAuditableEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity,bool>>? Criteria { get; set; } // P => P.Id == 1 // notic that cirtria represent Where condition ,and where does not deal with predicate 

        public List<Expression<Func<TEntity, object>>> Includes { get; set; } // object to can include any related entity,to use include with category or barand , order
    
        public Expression<Func<TEntity, object>>? OrderBy { get; set; } // both prodcts are nullable cuz we will sort against one of them
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }


        //if we will use then by , it will be list of expressions


        //pagination
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
