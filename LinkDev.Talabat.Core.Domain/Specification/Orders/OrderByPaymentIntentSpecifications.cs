using LinkDev.Talabat.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specification.Orders
{
    public class OrderByPaymentIntentSpecifications : BaseSpecification<Order, int>
    {
        public OrderByPaymentIntentSpecifications(string paymentIntentId)
            : base(order => order.PaymentIntendId == paymentIntentId)
        {
            
        }
    }
}
