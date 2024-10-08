using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,IConfiguration configuration ) // extension method for IServiceCollection to add the DbContext to DI container.
        {
            services.AddDbContext<StoreContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("StoreContext"));
            }/*,contextLifetime: ServiceLifetime.Scoped,optionsLifetime: ServiceLifetime.Scoped */);
            
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork)); 



            services.AddScoped<IStoreContextInitializer, StoreContextInitializer>(); // Register StoreContextInitializer To DI Container.


            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(BasedAuditableEntityInterceptor));

            return services;
        }
    }
}
