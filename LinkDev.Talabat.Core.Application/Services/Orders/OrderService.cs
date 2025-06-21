using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Basket;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Orders;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Orders
{
    internal class OrderService(IUnitOfWork unitOfWork,IMapper mapper,IBasketService basketService) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order)
        {
            // 1. Get Basket From Basket Repoo

            var basket = await basketService.GetCustomerBasketAsync(order.BasketId);

            //2. Get Selected Items at Basket From Products Repoo

            var orderItems = new List<OrderItem>();

            if (basket.Items.Count > 0)
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);

                    if (product is not null)
                    {
                        var productItemOrderd = new ProductItemOrderd()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            PictureUrl = product.PictureUrl ?? "",
                        };

                        var orderItem = new OrderItem()
                        {
                            Product = productItemOrderd,
                            Price = product.Price,
                            Quantity = item.Quantity
                        };

                        orderItems.Add(orderItem);
                    }


                }

            }

            // 3. calculate subtotal

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            //4. Map Address

            var address = mapper.Map<Address>(order.ShippingAddress);

            //4. Create Order

            var orderToCreate = new Order()
            {
                BuyerEmail = buyerEmail,
                ShippingAddress = address,
                Items = orderItems,
                Subtotal = subtotal,
                DeliveryMethodId = order.DeliveryMethodId,
            };

            await unitOfWork.GetRepository<Order, int>().AddAsync(orderToCreate);

            //5. save to database

            var created  = await unitOfWork.CompleteAsync() > 0;  // three rows will affected of we have order with two order items , two rows for items , one for order

            if (!created) throw new BadRequestException("an error has occurred during creating the order.");

            return mapper.Map<OrderToReturnDto>(orderToCreate);


        }

        public Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
