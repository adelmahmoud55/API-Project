using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction.Basket;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Basket;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructre;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Basket
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper,IConfiguration configuration) : IBasketService
    {

        public async Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId)
        {
           var basket = await basketRepository.GetAsync(basketId);

            if(basket is null) throw new NotFoundException(nameof(CustomerBasket), basketId);

            return mapper.Map<CustomerBasketDto>(basket);


        }

        public async Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);

            var TimeToLive = TimeSpan.FromDays(double.Parse(configuration.GetSection("RedisSettings")["TimeToLiveInDays"]!));

            var updatedBasket = await basketRepository.UpdateAsync(basket, TimeToLive);

            if(updatedBasket is null) throw new BadRequestException("Cannot update, there is a problem with this basket.");

            return basketDto;
        }
    
        public async Task DeleteCustomerBasketAsync(string basketId)
        {
           var deleted = await basketRepository.DeleteAsync(basketId);

            if(!deleted) throw new BadRequestException("unable to delete this basket.");

           
        }
    }
}
