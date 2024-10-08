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
        public Expression<Func<TEntity,bool>>? Criteria { get; set; } // P => P.Id == 1

        public List<Expression<Func<TEntity, object>>> Includes { get; set; } // object to can include any related entity
    }
}
