﻿using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
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
            services.AddAutoMapper(typeof(MappingProfile)); // IF U HAVE ONE PROFILE
            /*services.AddAutoMapper(typeof(MappingProfile).Assembly);*/ // IF U HAVE MORE THAN ONE PROFILE

            services.AddScoped(typeof(IServiceManager),typeof(ServiceManager));
            return services;
        }
    }
}
