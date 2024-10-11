using AutoMapper;
using LinkDev.Talabat.Core.Application.Products.Models;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    internal class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductToReturnDto, string?>
    {
        

        public string? Resolve(Product source, ProductToReturnDto destination, string? destMember, ResolutionContext context)
        {
            if(!string.IsNullOrWhiteSpace(source.PictureUrl))
                // here we are making the base url of the api dynamic by getting it from the appsettings.json file, which is changed from environment to another
                return $"{configuration["Urls:ApiBaseUrl"]}/{source.PictureUrl}";

            return string.Empty;
        }
    }
}
