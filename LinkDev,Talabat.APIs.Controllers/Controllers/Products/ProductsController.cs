using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Application.Abstaction.Products;
using LinkDev_Talabat.APIs.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev_Talabat.APIs.Controllers.Controllers.Products
{
    public class ProductsController(IServiceManager serviceManager) : ApiControllerBase
    {

        // get endpoints takes parameters from query string only{?id=1 from url } , lw htst2blhom k paramter lw7do tmam , lw hts2bl k object lazm tst5dm [FromQuery] attribute,3shan t3rf ef enk htst2bl el object k query string
        [HttpGet] // Get: api/Products   
        public async Task<ActionResult> GetProducts([FromQuery]ProductSepcParams SepcParams) 
        {
            var products = await serviceManager.ProductService.GetProductsAsync(SepcParams);
            return Ok(products);
        }

        [HttpGet("{id:int}")] // Get: api/Products/id
        public async Task<ActionResult> GetProduct(int id)
        {
            var products = await serviceManager.ProductService.GetProductAsync(id);

            if (products == null)
                return NotFound(new {statusCode =404, message="not found"});
            
            return Ok(products);
        }


        //("brands") to make the route more clear, w tfr2 benha w ben ba2y el routes
        [HttpGet("brands")] // Get: api/Products/brands
        public async Task<ActionResult> GetBrands()
        {
            var brands = await serviceManager.ProductService.GetBrandsAsync();
            return Ok(brands);
        }


        [HttpGet("categories")] // Get: api/Products/categories
        public async Task<ActionResult> GetCategories()
        {
            var categories = await serviceManager.ProductService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}

