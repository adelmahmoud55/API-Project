using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
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
            #region Store DbContext

            services.AddDbContext<StoreDbContext>(optionsBuilder =>
             {
                 optionsBuilder
                 .UseLazyLoadingProxies()
                 .UseSqlServer(configuration.GetConnectionString("StoreContext"));
             }/*,contextLifetime: ServiceLifetime.Scoped,optionsLifetime: ServiceLifetime.Scoped */);

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));



            services.AddScoped<IStoreContextInitializer, StoreDbContextInitializer>(); // Register StoreContextInitializer To DI Container.


            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(BasedAuditableEntityInterceptor)); // hena enta bt2olo and bst5dm BasedAuditableEntityInterceptor l2no by default hwa byst5dm savechangesinterceptor  


            #endregion

            #region Identity DbContext

            services.AddDbContext<StoreIdentityDbContext>(optionsBuilder =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            }/*,contextLifetime: ServiceLifetime.Scoped,optionsLifetime: ServiceLifetime.Scoped */);





            #endregion



            return services;
        }
    }
}
