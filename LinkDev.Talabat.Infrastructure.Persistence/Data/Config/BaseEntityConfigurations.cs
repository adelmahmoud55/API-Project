
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Config
{
    internal class BaseEntityConfigurations<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd(); // AUTO INCREMENT IF NUMERIC (1,1) OR GUID like UseIdentityColumn();, used when the primary key its type will change(generic) from entity to entity




            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.CreatedOn)
                .IsRequired()
                /*.HasDefaultValueSql("GETUTCDATE()")*/;
                
            
            builder.Property(e => e.LastModifiedBy)
                .IsRequired();


            builder.Property(e => e.LastModifiedOn)
            .IsRequired()
                   /*.HasDefaultValueSql("GETUTCDATE()")*/;

        }

     
    }
}
