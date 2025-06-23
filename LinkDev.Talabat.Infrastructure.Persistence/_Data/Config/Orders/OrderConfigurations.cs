using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Config.Orders
{
    internal class OrderConfigurations : BaseAuditableEntityConfigurations<Order,int>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(order => order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(order => order.Status)
                .HasConversion
                (

                     (oStatus) => oStatus.ToString(),
                     (oStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), oStatus)
               
                );


            builder.Property(order => order.Subtotal)
                .HasColumnType("decimal(8,2)");
            
            builder.HasOne(order => order.DeliveryMethod)
                .WithMany()
                .HasForeignKey(order => order.DeliveryMethodId)
                .OnDelete(DeleteBehavior.SetNull); 


            builder.HasMany(order => order.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
