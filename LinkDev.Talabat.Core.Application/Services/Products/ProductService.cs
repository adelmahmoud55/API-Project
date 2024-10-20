using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction.Comman;
using LinkDev.Talabat.Core.Application.Abstaction.Products;
using LinkDev.Talabat.Core.Application.Products;
using LinkDev.Talabat.Core.Application.Models.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specification;
using LinkDev.Talabat.Core.Domain.Specification.Products_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkDev.Talabat.APIs.Controllers.Exceptions;

namespace LinkDev.Talabat.Core.Application.Services.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
            => mapper.Map<IEnumerable<BrandDto>>(await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync());



        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
            => mapper.Map<IEnumerable<CategoryDto>>(await unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync());

    
        public async Task<ProductToReturnDto> GetProductAsync(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetWithSpecAsync(specs);

            if (product is null)
                throw new NotFoundException(nameof(Product), id); //this will be handled in the middleware   app.UseStatusCodePagesWithReExecute("/Error/{0}"); 

            var mappedProduct = mapper.Map<ProductToReturnDto>(product);

            return mappedProduct;
        }


        public async Task<Pagination<ProductToReturnDto>> GetProductsAsync(ProductSepcParams sepcParams)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(sepcParams.Sort, sepcParams.BrandId, sepcParams.CategoryId, sepcParams.PageSize, sepcParams.PageIndex, sepcParams.Search);

            var products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(specs); // IEnumerable<Product> 

            var data = mapper.Map<IEnumerable<ProductToReturnDto>>(products);



            var countspecs = new ProductWithFilterationForCountSpecifications(sepcParams.BrandId, sepcParams.CategoryId, sepcParams.Search);

            //to get count u need another query , with diff spec  
            var count = await unitOfWork.GetRepository<Product, int>().GetCountAsync(countspecs); // same object will be used of unitOfWork cuz its the same request, we work with concurrent data structure

            return new Pagination<ProductToReturnDto>(sepcParams.PageSize, sepcParams.PageIndex, count) { Data = data };
            
        }
    }
}
