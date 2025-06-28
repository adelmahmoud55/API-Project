using AutoMapper;
using LinkDev.Talabat.APIs.Controllers.Exceptions;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Basket;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Orders;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructre;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specification.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Orders
{
    internal class OrderService(IUnitOfWork unitOfWork,IMapper mapper,IBasketService basketService,IPaymentService paymentService) : IOrderService
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



            // 5. Get Delivery Method

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(order.DeliveryMethodId);



            //6. Create Order

                var orderRepo = unitOfWork.GetRepository<Order, int>();

                var orderSpecs = new OrderByPaymentIntentSpecifications(basket.PaymentIntentId!);

                var existingOrder = await orderRepo.GetWithSpecAsync(orderSpecs);

                if (existingOrder is not null)
                {
                    orderRepo.Delete(existingOrder);
                    await paymentService.CreateOrUpdatePaymentIntent(basket.Id); 

                }


            var orderToCreate = new Order()
            {
                BuyerEmail = buyerEmail,
                ShippingAddress = address,
                Items = orderItems,
                Subtotal = subtotal,
                DeliveryMethod = deliveryMethod,
                PaymentIntentId = basket.PaymentIntentId!
            };

            await orderRepo.AddAsync(orderToCreate);






            //7. save to database

            var created  = await unitOfWork.CompleteAsync() > 0;  // three rows will affected of we have order with two order items , two rows for items , one for order

            if (!created) throw new BadRequestException("an error has occurred during creating the order.");

            return mapper.Map<OrderToReturnDto>(orderToCreate);


        }

        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderSpecs = new OrderSpecifications(buyerEmail);

            var orders = await unitOfWork.GetRepository<Order,int>().GetAllWithSpecAsync(orderSpecs);

            return mapper.Map<IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
           var orderSpecs = new OrderSpecifications(buyerEmail, orderId);

            var order =await  unitOfWork.GetRepository<Order, int>().GetWithSpecAsync(orderSpecs);
          
            if (order is null ) throw new NotFoundException(nameof(Order), orderId);

            return mapper.Map<OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

            return mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }


    }
}
