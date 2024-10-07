
using LinkDev.Talabat.Infrastructure.Persistence;

namespace LinkDev.Talabat.APIs
{
    public class Program
    {
        // Entry point for the application.
        public static void Main(string[] args)
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
