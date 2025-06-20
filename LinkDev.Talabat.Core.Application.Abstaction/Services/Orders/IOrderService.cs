using LinkDev.Talabat.Core.Application.Abstaction.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order);

        Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId);

        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail);

        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
    }
}
