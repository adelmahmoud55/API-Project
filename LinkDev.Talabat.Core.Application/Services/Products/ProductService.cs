using AutoMapper;
using LinkDev.Talabat.Core.Application.Products;
using LinkDev.Talabat.Core.Application.Products.Models;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specification;
using LinkDev.Talabat.Core.Domain.Specification.Products_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var mappedProduct = mapper.Map<ProductToReturnDto>(product);

            return mappedProduct;
        }


        public async Task<IEnumerable<ProductToReturnDto>> GetProductsAsync()
        {
            var specs = new ProductWithBrandAndCategorySpecifications();

            var products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(specs);

            var mappedProducts = mapper.Map<IEnumerable<ProductToReturnDto>>(products);

            return mappedProducts;
        }
    }
}
