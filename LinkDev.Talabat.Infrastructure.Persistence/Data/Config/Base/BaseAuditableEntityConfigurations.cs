using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base
{
    internal class BaseAuditableEntityConfigurations<TEntity, TKey> : BaseEntityConfigurations<TEntity, TKey>
        where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
           

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
