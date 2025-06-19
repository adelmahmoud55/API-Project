using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Products
{
    internal class BrandConfigurations : BaseAuditableEntityConfigurations<ProductBrand, int>
    {
        public override void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            base.Configure(builder);

            builder.Property(B => B.Name)
                .IsRequired();
                
        }
    }
}
