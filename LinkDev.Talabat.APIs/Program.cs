
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

            webApplicationBuilder.Services.AddControllers(); // Register Required Services By ASP.NET Core Web APIs To Di Container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();


            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration); 
            //DependencyInjection.AddPersistenceServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration); // Register Persistence Services To DI Container.



            #endregion

            var app = webApplicationBuilder.Build();

           using  var Scope =  app.Services.CreateAsyncScope();
                  var Services = Scope.ServiceProvider;
                  var dbContext = Services.GetRequiredService<StoreContext>();
                   // ask runtime enviroment  for an object from "StoreContext" Service explicitly.
                    
                  var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try 
            {
                var PendingMigrations = dbContext.Database.GetPendingMigrations(); // Get Pending Migrations.
                //we can use MigrateAsync direct but time consum


                if (PendingMigrations.Any())
                    await  dbContext.Database.MigrateAsync(); // Update Database 
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during applying the migratings.");
            }

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
