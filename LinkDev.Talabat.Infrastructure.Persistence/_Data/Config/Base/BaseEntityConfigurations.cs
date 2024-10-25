using LinkDev.Talabat.Infrastructure.Persistence._Comman;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base
{
    
    [DbContextType(typeof(StoreDbContext))] //no need to type the DbContextTypeAttribute, it will be added automatically, use just the type property, this for the reflection to get the type of the DbContext
    internal class BaseEntityConfigurations<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd(); // AUTO INCREMENT IF NUMERIC (1,1) OR GUID like UseIdentityColumn();, used when the primary key its type will change(generic) from entity to entity
        }    
    }
}
