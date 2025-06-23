using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Basket;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Comman;
using LinkDev.Talabat.Core.Application.Abstaction.Models.Orders;
using LinkDev.Talabat.Core.Application.Models.Products;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderAddress = LinkDev.Talabat.Core.Domain.Entities.Orders.Address;
using UserAddress = LinkDev.Talabat.Core.Domain.Entities.Identity.Address;


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

                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>()); // this generic method is used to resolve the picture url  , and must take an object implementing IValueResolver interface 

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();


            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod!.ShortName));


            CreateMap<OrderItem, OrderItemDto>()
               .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.ProductId))
               .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
               .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

            CreateMap<UserAddress, AddressDto>().ReverseMap();

        }
    }
}