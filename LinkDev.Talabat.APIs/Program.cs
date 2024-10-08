using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.APIs
{
    public class Program
    {
        // Entry point for the application.
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);


            #region Configure Services

            // Add services to the container.

            //AddApplicationPart is used to add the controllers to the DI container. and to tell that the controllers are in the same assembly as the AssemblyInformation class.
            webApplicationBuilder.Services.AddControllers().AddApplicationPart(typeof(LinkDev.Talabat.APIs.Controllers.AssemblyInformation).Assembly); // Register Controllers To DI Container.
                                                                                                                                                       // Register Required Services By ASP.NET Core Web APIs To Di Container.

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();


            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            //DependencyInjection.AddPersistenceServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration); // Register Persistence Services To DI Container.

            webApplicationBuilder.Services.AddApplicationServices(); // Register Application Services To DI Container.

            //webApplicationBuilder.Services.AddScoped(typeof(IHttpContextAccessor),typeof(HttpContextAccessor));
            webApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService)); // Register LoggedInUserService To DI Container.

            #endregion

            var app = webApplicationBuilder.Build();

            #region Update DataBase and Data Seeding
           
           await  app.InitializerStoreContextAsync();
           
            #endregion

            #region Configure Kestrel Middlewares

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization(); 

            app.MapControllers();

            #endregion




            app.Run();
        }
    }
}
