﻿using LinkDev.Talabat.Core.Application.Abstaction;
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

        [HttpGet] // Get: api/Products
        public async Task<ActionResult> GetProducts()
        {
            var products = await serviceManager.ProductService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")] // Get: api/Products/id
        public async Task<ActionResult> GetProduct(int id)
        {
            var products = await serviceManager.ProductService.GetProductAsync(id);

            if (products == null)
                return NotFound();
            
            return Ok(products);
        }

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

