using LinkDev.Talabat.Core.Application.Abstaction.Comman.Contracts.Infrastructure;
using LinkDev.Talabat.Shared.Models.Basket;
using LinkDev_Talabat.APIs.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Payment
{
 
    public class PaymentController(IPaymentService paymentService) : ApiControllerBase
    {
        [Authorize]
        [HttpPost("{basketId}")] // Post api/payment/{basketId}
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
            return Ok(result);
        }

        [HttpPost("webhook")] // Post api/payment/webhook
        public async Task<IActionResult> WebHook()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
            await paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);

            return Ok();
        }

    }
}