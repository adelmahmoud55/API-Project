using LinkDev.Talabat.Core.Application.Abstaction.Comman.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Application.Abstaction.Services;
using LinkDev.Talabat.Shared.Models.Basket;
using LinkDev_Talabat.APIs.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Basket
{
    public class BasketController(IBasketService basketService) : ApiControllerBase
    {
        [HttpGet] // Get: api/Basket?id=
        public async Task<ActionResult> GetBasket(string id)
        {
            var basket = await basketService.GetCustomerBasketAsync(id);
            return Ok(basket);
        }


        [HttpPost] // Post: api/Basket
        public async Task<ActionResult> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = await basketService.UpdateCustomerBasketAsync(basketDto);
            return Ok(basket);
        }


        [HttpDelete] // Delete: api/Basket
        public async Task DeleteBasket(string id)
        {
            await basketService.DeleteCustomerBasketAsync(id);
            
        }
    }
}
