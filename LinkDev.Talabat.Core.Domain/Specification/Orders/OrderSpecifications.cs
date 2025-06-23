using LinkDev.Talabat.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specification.Orders
{
    public class OrderSpecifications : BaseSpecification<Order,int>
    {

        public OrderSpecifications(string buyerEmail, int orderId)
          : base(order => order.Id == orderId && order.BuyerEmail == buyerEmail)
        {
            AddIncludes();
          
        }



        public OrderSpecifications(string buyerEmail)
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddIncludes();
            AddOrderByDesc(order => order.OrderDate);
        }


        private protected override void AddIncludes()
        {
            base.AddIncludes();


            // loading related entities eagerly 3shan kol mra brg3 order lazm yb2a f itmes mfesh order mn 8er items, for virtials properties
            Includes.Add(order => order.Items);
            Includes.Add(order => order.DeliveryMethod!);
        }

    }
}
