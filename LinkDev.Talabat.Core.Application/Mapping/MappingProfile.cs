using AutoMapper;
using LinkDev.Talabat.Core.Application.Products.Models;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductCategory, CategoryDto>();
            //CreateMap<Product, ProductToReturnDto>()
            //    .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand))
            //    .ForMember(d => d.Category, o => o.MapFrom(s => s.Category));
        }
    }
}
