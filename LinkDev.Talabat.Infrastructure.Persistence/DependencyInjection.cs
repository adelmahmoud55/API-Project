using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitializer;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
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



            services.AddScoped<IStoreDbInitializer, StoreDbInitializer>(); // Register StoreContextInitializer To DI Container.


            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(BasedAuditableEntityInterceptor)); // hena enta bt2olo and bst5dm BasedAuditableEntityInterceptor l2no by default hwa byst5dm savechangesinterceptor  


            #endregion

            #region Identity DbContext

            services.AddDbContext<StoreIdentityDbContext>(optionsBuilder =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            }/*,contextLifetime: ServiceLifetime.Scoped,optionsLifetime: ServiceLifetime.Scoped */);

            services.AddScoped<IStoreIdentityDbInitializer, StoreIdentityDbInitializer>(); // Register IdentityContextInitializer To DI Container.


            //services.AddScoped(typeof(UserManager<ApplicationUser>));
            //services.AddIdentityCore<ApplicationUser>();
            //  ana msh hfdl a3dy 3la kol service service f Identity package w a3mlha register f el DI container
            // 3ndy method esmha AddIdentityCore<> bt3ml register for usermangerservice b el dependency bt3tha
            // lakn msh h2dr a3ml register l services like sign in manager , 3shan ana hena f lower layer msh h3ml sign in
            // f 3shan kda el method eli esmha AddIdentity(); msh h3rf awslha hena 3shan ana msh f el layer eli by3ml sign in like presentation layer or api layer
            // f h3ml el registeration f el higher layer f el Api in program.cs
            // wkman 3shan adef el storeservice f el higher layer f el api layer
            // f AddIdentity dh b register kol el services b el dependency bt3tha


            #endregion




            return services;
        }
    }
}
