using AutoMapper;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Talabat.DashBoard.Models;

namespace Talabat.DashBoard.Helpers
{
    public class MapsProfile : Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductViewModel>()
    .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.Brand))
    .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Category))
    .ReverseMap()
    .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.ProductBrand))
    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ProductType));


            CreateMap<Product,ProductViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Category));
            CreateMap<ProductViewModel, Product>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.ProductBrand))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ProductType))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifiedOn, opt => opt.Ignore());
        }
    }
}
