using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Basket;
using LinkDev.Talabat.Core.Application.Models.Products;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
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
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand!.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category!.Name))
                //.ForMember(d => d.PictureUrl, o => o.MapFrom(s => $"{"https://localhost:7248"}{s.PictureUrl}"));

                .ForMember(d => d.PictureUrl, o => o.MapFrom< ProductPictureUrlResolver>()); // this generic method is used to resolve the picture url  , and must take an object implementing IValueResolver interface 
        
            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        
        
        }
    }
}
