﻿using AutoMapper;
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
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
            => _mapper.Map<IEnumerable<BrandDto>>(await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync());



        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
            => _mapper.Map<IEnumerable<CategoryDto>>(await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync());


        public async Task<ProductToReturnDto> GetProductAsync(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);

            var product = await _unitOfWork.GetRepository<Product, int>().GetWithSpecAsync(specs);

            var mappedProduct = _mapper.Map<ProductToReturnDto>(product);

            return mappedProduct;
        }


        public async Task<IEnumerable<ProductToReturnDto>> GetProductsAsync()
        {
            var specs = new ProductWithBrandAndCategorySpecifications();

            var products = await _unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(specs);

            var mappedProducts = _mapper.Map<IEnumerable<ProductToReturnDto>>(products);

            return mappedProducts;
        }
    }
}
