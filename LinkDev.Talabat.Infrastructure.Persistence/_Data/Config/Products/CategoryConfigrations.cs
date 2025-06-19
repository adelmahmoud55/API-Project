using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Products
{
    internal class CategoryConfigrations : BaseAuditableEntityConfigurations<ProductCategory, int>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);

            builder.Property(C => C.Name)
                .IsRequired();
        }
    }   
    

    
}
