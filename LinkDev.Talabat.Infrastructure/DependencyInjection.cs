using LinkDev.Talabat.Core.Domain.Contracts.Infrastructre;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LinkDev.Talabat.Infrastructure.BasketRepository;
using LinkDev.Talabat.Shared.Models;
using LinkDev.Talabat.Infrastructure.Payment_Service;
using LinkDev.Talabat.Core.Application.Abstaction.Comman.Contracts.Infrastructure;


namespace LinkDev.Talabat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) => 
            {
                var connectionString = configuration.GetConnectionString("Redis");
                var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString!);
                return connectionMultiplexer;
            });

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository.BasketRepository));

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));


            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));


            return services;
        }
    }
}