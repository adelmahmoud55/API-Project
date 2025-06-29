using AutoMapper;
using LinkDev.Talabat.APIs.Controllers.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructre;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specification.Orders;
using LinkDev.Talabat.Shared.Models;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = LinkDev.Talabat.Core.Domain.Entities.Products.Product;

namespace LinkDev.Talabat.Infrastructure.Payment_Service
{
    internal class PaymentService(
        IBasketRepository basketRepository, 
        IUnitOfWork unitOfWork, IMapper mapper,
        IOptions<RedisSettings> redisSettings,
        IOptions<StripeSettings> stripeSettings,
        ILogger<PaymentService> logger
        ) : IPaymentService
    {

        private readonly RedisSettings _redisSettings = redisSettings.Value;
        private readonly StripeSettings _stripeSettings = stripeSettings.Value;

        public async  Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {

            StripeConfiguration.ApiKey = _stripeSettings.Secretkey; //key needed to interact with Stripe API

            var basket = await basketRepository.GetAsync(basketId);

            if (basket is null) throw new NotFoundException(nameof(CustomerBasket), basketId);

           if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod is null) throw new NotFoundException(nameof(DeliveryMethod), basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
            }

            if(basket.Items.Count > 0)
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);
                    if (product is  null) throw new NotFoundException(nameof(Product), item.Id);
                    if(item.price != product.Price)
                        item.price = product.Price; // Update the price in the basket to match the product price

                }
            }

            PaymentIntent? paymentIntent = null;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // create new payment intent 
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(basket.Items.Sum(item => item.price * item.Quantity) + basket.ShippingPrice) * 100, // Convert to cents
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" },
                };
              
                paymentIntent = await paymentIntentService.CreateAsync(options); // interact with Stripe API to create a new payment intent
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // update existing payment intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(basket.Items.Sum(item => item.price * item.Quantity) + basket.ShippingPrice) * 100, // Convert to cents
                };
                 
               await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options); // interact with Stripe API to update the existing payment intent
            }


            await basketRepository.UpdateAsync(basket, TimeSpan.FromDays(_redisSettings.TimeToLiveInDays)); // Update the basket in Redis

             return mapper.Map<CustomerBasketDto>(basket); // Return the updated basket with payment intent details
             
        }

        public async Task UpdateOrderPaymentStatus(string requestBody, string Headr)
        {
            var stripeEvent = EventUtility.ConstructEvent(requestBody, Headr, _stripeSettings.WebhookSecret);


            // Handle the event based on its type

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            Order? order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                   order = await UpdatePaymentIntent(paymentIntent.Id, ispaid: true);
                    logger.LogInformation("ORDER is Succeede with Payment Intent: {0}", paymentIntent.Id);
                    break;
                case "payment_intent.payment_failed":
                    order = await UpdatePaymentIntent(paymentIntent.Id, ispaid: false);
                    logger.LogInformation("ORDER is not Succeede with Payment Intent: {0}", paymentIntent.Id);

                    break;
            }



        }

        private  async Task<Order> UpdatePaymentIntent(string  paymentIntentId , bool ispaid)
        {
            var orderRepo = unitOfWork.GetRepository<Order, int>();

            var specs = new OrderByPaymentIntentSpecifications(paymentIntentId);

            var order = await orderRepo.GetWithSpecAsync(specs); 

            if(order is null) throw new NotFoundException(nameof(Order), $"PaymentIntentId: {paymentIntentId}");

            if(ispaid)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            orderRepo.Update(order);

            await unitOfWork.CompleteAsync(); // Save changes to the database

            return order;
        }
    }
} 
