using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Basket;
using LinkDev_Talabat.APIs.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Basket
{
    public class BasketController(IServiceManager serviceManager) : ApiControllerBase
    {
        [HttpGet] // Get: api/Basket?id=
        public async Task<ActionResult> GetBasket(string id)
        {
            var basket = await serviceManager.BasketService.GetCustomerBasketAsync(id);
            return Ok(basket);
        }


        [HttpPost] // Post: api/Basket
        public async Task<ActionResult> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = await serviceManager.BasketService.UpdateCustomerBasketAsync(basketDto);
            return Ok(basket);
        }


        [HttpDelete] // Delete: api/Basket
        public async Task DeleteBasket(string id)
        {
            await serviceManager.BasketService.DeleteCustomerBasketAsync(id);
            
        }
    }
}
