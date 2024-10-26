﻿using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction.Services;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Basket;
using LinkDev.Talabat.Core.Application.Services.Basket;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructre;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            // IF U HAVE ONE PROFILE :
            services.AddAutoMapper(typeof(MappingProfile)); // w hena hya b create object lw7do mn el mapping profile parameterless constructor
            /* services.AddAutoMapper(Mapper => Mapper.AddProfile(new MappingProfile()));*/ // hena nfc el klam bs momkn ast5dmha lw el constructor takes parameter


            // IF U HAVE MORE THAN ONE PROFILE
            /*services.AddAutoMapper(typeof(MappingProfile).Assembly);*/

            services.AddScoped(typeof(IServiceManager),typeof(ServiceManager));



            // we cannot use AddScoped for BasketService with overload  because it has dependencies that are not registered in the DI container.
            //services.AddScoped(typeof(IBasketService), typeof(BasketService));
            //services.AddScoped(typeof(Func<IBasketService>), typeof(Func<BasketService>));



            // Custom Instantiation Needs , The DI container's role is to resolve the dependencies.
            services.AddScoped(typeof(Func<IBasketService>), (ServiceProvider) =>
            {
                // we can use ServiceProvider to get the dependencies that are not registered in the DI container , to help the DI to create the object.
                var mapper = ServiceProvider.GetRequiredService<IMapper>();
                var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                var basketRepository = ServiceProvider.GetRequiredService<IBasketRepository>();

                return () => new BasketService(basketRepository, mapper, configuration); // DI will not create BasketService object , we are creating it here.
            });


            return services;
        }
    }

    #region Auto Mapper with DI Container 
    /*
     When the DI container needs to create an instance of a class that depends on MappingProfile,
    it will use the parameterless constructor of MappingProfile to create the object. 
    This means that the DI container relies on the parameterless constructor to instantiate the MappingProfile object.
    However, it's important to note that this behavior is specific to the AddAutoMapper method and may not apply to other DI container configurations or scenarios.
    In general, DI containers can be configured to use different strategies for object creation, including constructor injection with parameters.

     */

    #endregion
}
